# Terraria-1.4.5.8-decomp-recomp

> 語言： [English](README.md) | [繁體中文](README.zh-TW.md)

把一份**反編譯**的 C# 遊戲程式碼（Terraria 1.4.5.8，.NET Framework 4.7.2，x86）
重新建置成可執行檔，再修掉過程中冒出來的五個缺陷。
五個全部由反編譯器引入，**全部能編譯通過、全部沒有編譯警告**。

> 純文字文件，不含任何圖片或遊戲素材。

---

## 目錄

- [五個 bug](#五個-bug)
- [損傷速查表](#損傷速查表)
- [怎麼找到的](#怎麼找到的)
- [建置](#建置)
- [編譯警告](#編譯警告)
- [倉庫不含什麼](#倉庫不含什麼)
- [免責聲明](#免責聲明)

---

## 五個 bug

### 1 — 虛擬派發被翻譯成自我呼叫

讀任何世界都立刻 `StackOverflowException`。

反編譯器把 `((Game)this).Update(gameTime)` 正規化成 `this.Update(gameTime)`。
`Main` 覆寫了 `Update`，於是無限遞迴。實際量到 **64,526 層** `Main.Update`。

```diff
- this.Update(gameTime);        + base.Update(gameTime);
- this.Update(new GameTime());  + base.Update(new GameTime());
- this.Initialize();            + base.Initialize();
- this.Draw(gameTime);          + base.Draw(gameTime);     // ×2
- this.EndDraw();               + base.EndDraw();
```

六處全在同一個檔案（`Main.cs`）。審查時看不出問題——`this.X()` 太正常了。

### 2 — 例外處理器自我遞迴 → 行程卡死

症狀是「無回應、吃 CPU」，不是崩潰，所以躲過了正常除錯。

```csharp
AppDomain.CurrentDomain.FirstChanceException += (sender, args) =>
{
    var trace = new StackTrace(1, true).ToString();   // ← 可能拋出
    var text  = PrintException(args.Exception, trace); // ← 可能拋出
};
```

處理器負責**回報**例外；回報拋出的例外又觸發 `FirstChanceException`，
重新進入同一個處理器，再拋一次。無限倒退。

修法是 `[ThreadStatic]` 再進入防護加 `try/catch/finally`。
這一修也**順帶揭露了 bug 4**——原本失控的處理器一直在吞掉真正的例外。

### 3 — 遺失 PE 旗標 → 32 位元位址空間耗盡

只有大世界會炸，`OutOfMemoryException`，位置在 `WorldGen.clearWorld()` 第 7486 行
的 `Main.tile[l, m] = new Tile()`。

重建時丟掉了 COFF characteristics 裡的 `IMAGE_FILE_LARGE_ADDRESS_AWARE`：

```text
原始版本  Characteristics=0x0122   LARGE_ADDRESS_AWARE=True    → 4 GB
重建版本  Characteristics=0x0102   LARGE_ADDRESS_AWARE=False   → 2 GB
```

6400×1800 = **1,152 個格子單位萬**，逐一 `new Tile()`，2 GB 不夠，4 GB 綽綽有餘。

這旗標不屬於受管理中繼資料，編譯器不會警告，而且**每次重新建置都會遺失**。

### 4 — 空值檢查被翻譯成強轉 → 世界生成靜默中止

按「Create New World」，進度條從頭到尾不出現。沒有崩潰、沒有對話框，
只有一個看起來壞掉的按鈕。

```diff
- if ((int)biomes == 0)     + if (biomes == null)
      biomes = new JObject();     biomes = new JObject();
```

`JToken` 有 `explicit operator int`，所以 `(int)biomes` **會編譯**，
執行期丟 `ArgumentException: Can not convert Object to Int32`。
世界生成死在第一行，還沒來得及回報任何進度——而進度條正是它驅動的。

### 5 — 12 個語言檔被誤判為附屬組件 → UI 顯示原始鍵值

介面文字顯示成 `UI.Workshop` 而不是 `Workshop`。

```text
bin/Release/net472/en-US/Terraria.resources.dll     (138 KB)   ← 12 個檔案在這
bin/Release/net472/zh-Hans/Terraria.resources.dll   (136 KB)
```

MSBuild 的文化推斷看到檔名裡的 `Content.**en-US**.json`，判定為在地化資源，
輸出成附屬組件。分卷檔（`.Game.json`、`.Items.json`…）檔名沒有裸露的文化代碼，
所以正常內嵌——這就是為什麼**有些**文字對、**有些**不對。

遊戲只從主組件讀，於是那 12 個主要語言檔形同隱形。

```diff
- <EmbeddedResource Include="...en-US.json" LogicalName="...en-US.json" />
+ <EmbeddedResource Include="...en-US.json" LogicalName="...en-US.json" WithCulture="false" />
```

資訊清單資源 **88 → 100**，與參考執行檔一致。

---

## 損傷速查表

全部能編譯，全部無警告。

| 損傷型態 | 失敗方式 |
| ---- | ---- |
| `((Base)this).M()` → `this.M()` | 無限遞迴 |
| `ldnull`+`ceq` → `(int)obj == 0` | 執行期 `ArgumentException` |
| 枚舉比較 → `(int)enumVal == 1` | **無害**，語意正確 |
| 空白 case → `switch (x) {}` | **無害**，no-op |
| 資源文化推斷 → 附屬組件 | 資料被無聲搬走 |
| COFF characteristics 遺失 | 位址空間砍半 → OOM |

中間兩列同樣重要：全部標紅和全部不標，一樣沒用。
判斷哪個 `(int)x == 0` 是缺陷、哪個其實是枚舉測試，得先知道 `x` 的型別。

---

## 怎麼找到的

| 手段 | 抓到幾個 |
| ---- | ---- |
| ClrMD（x86 附加暫停，傾印每個執行緒呼叫堆疊） | bug 1、2 |
| Windows 應用程式事件記錄（`0xc00000fd` = 堆疊溢位） | bug 1 的性質確認 |
| 遊戲自帶 `-logerrors`／`-logfile`（first-chance 例外） | bug 2、4、5 |
| PE 標頭與資訊清單資源表直接比對 | bug 3、5 |
| 中繼資料比對（型別／欄位／方法／屬性／事件） | 0 |
| 靜態掃描（自我呼叫、參考型別非法強轉） | 0 |
| **編譯警告** | **0** |

後三項只縮小範圍，一個實質 bug 都沒抓到。

---

## 建置

```bash
dotnet build -c Release
```

**0 錯誤 / 633 警告。** 出貨前需補 `LARGEADDRESS_AWARE`（見 bug 3），
此步驟無法由編譯器完成。

---

## 編譯警告

| 代碼 | 數量 | 佔比 | 判定 |
| ---- | ---- | ---- | ---- |
| CS0618 | 1216 | 96.1% | 過時 API，刻意使用 — 不要動 |
| CS0649 | 18 | 1.4% | 誤報，欄位由 JSON 反序列化填入 |
| CS0169 | 12 | 0.9% | 私有欄位未讀取 |
| CS0067 | 8 | 0.6% | 事件僅由框架反射掛接 |
| CS0219 | 6 | 0.5% | 賦值後未使用 |
| CS0414 | 2 | 0.2% | 賦值後未讀取 |
| CS1522 | 2 | 0.2% | 空白 `switch`，已確認 no-op |
| CS9113 | 2 | 0.2% | 未使用參數 |

逐一稽核只查出一個真產物：有讀取但從未賦值的著色器飽和度欄位，恆為零（外觀問題）。

五個實質 bug 沒產生任何一條警告。警告數量在反編譯程式碼裡
是執行期正確性的一個很差勁的代理指標。

---

## 倉庫不含什麼

- 遊戲素材：無圖片、貼圖、音訊、字型
- 出貨執行檔：不分發任何已編譯的遊戲程式
- 本地化資料：語言 JSON 未附，僅說明引用它的建置設定
- 專有文本：物品名稱、對話、背景設定一概不予重現

---

## 免責聲明

Terraria 版權所有 © Re-Logic LLC。*Terraria* 為 Re-Logic LLC 之商標。
本專案與 Re-Logic LLC、Team Terraria 無任何關聯，亦未獲背書或贊助。

這是一項獨立、非營利的**教育性質**研究，記錄一類軟體工程上的失誤及其偵測方法。
其中不含遊戲素材、執行檔與具份量的著作權保護文本。
