photo happy photo   v1.0.0
# 简述
玩家需要操纵摄影师为一群人拍合影，为了让照片完美，摄影师必须保证拍照的瞬间所有人都在笑；不同的人会因为各种原因不保持笑脸，摄影师只能走到他们面前使用相应道具哄得他们大笑。

游戏目标是让在拍照的瞬间有尽可能多的人在保持笑的状态。

# 场景
主视角是从摄像机后方正对着人群，摄影师可以从摄像机处移动到人群中，并且通过携带和使用道具来安抚对应人员，人群在阶梯或斜坡上站成一排一排，即类似合影时的站法。


空间关系示意图：

人 人 通道 人 人 人 通道 人 人  ->第三行

————————————————

人 人 通道 人 人 人 通道 人 人  ->第二行

————————————————

人 人 通道 人 人 人 通道 人 人  ->第一行

————————————————

空地 通道 空地 空地 通道 空地  -> 第零行

————————————————

中间地带 通道 中间地带 通道 中间地带

————————————————

摄像机 - UI

——————————
# 交互逻辑

## 摄影师：
### 1-移动
摄影师可以通过wasd进行移动。在从摄影机移动向人群的过程中，摄影师可以在xz平面上任意地进行移动；但在人群中时，因为人群是固定站成一排一排的，所以在某一行的人员前移动时，摄影师只能左右移动（x轴上），即只能在这行人员面前移动，在没有人的地方摄影师才能移动到其他行上（在z轴上移动）。

WASD-移动键

### 2-交互
面对不配合的人员，摄影师必须使用对应的方法和他们交互才能安抚他们。
空手时，使用e键进行交互。摄影师需要走到目标附近，做出对应操作或者使用对应道具，才能令目标暂时转换成笑脸的状态。

E-交互键

### 3-持有
摄影师只有两只手，因此最多同时持有两个道具，j使用左手道具，l使用右手道具，i交换两只手。有些交互必须空手进行；进行拍摄时，必须两只手上都没有道具。

J/L-道具键

### 4-拾取
面对没被持有的道具，摄影师可以使用e键和附近的道具交互，将最近的道具拾起。若拾起时手上已经有道具，则自动将右手手上道具向下丢弃（投掷）。

### 5-投掷
摄影师可以使用k键将右手道具掷出；s/↓+k将道具原地丢弃，不使用方向键时k键将道具往摄影师面向方向投掷，w/↑+k将道具向摄影师面向方向的斜上方投掷。（道具在空中时也可以被捡起）

K-投掷键

### 6-拍摄
摄影师操纵摄像机时必须来到摄像机处（即镜头外），操作拍摄键Space，进入拍摄倒计时，倒计时结束时拍照并结算分数。

Space-拍摄键

## 合影人员：
### 1-笑的状态
合影人员不会长期保持笑的状态，随着时间的增长，就有越大概率变为其他状态；合影人员有时也会受到其他合影人员的状态的影响，导致其脱离笑的状态。在该状态下，合影人员的位置和动作是固定不动的。

### 2-其他状态
不同类型的合影人员的非笑状态是不同的，详细见后文。处于其他状态的合影人员需要摄影师进行对应操作，才能将其转变为笑的状态。

### 3-合影人员类型
1、瞌睡虫

2、社畜

3、婴儿

4、街机小子

5、母亲

6、父亲

7、猫

## 道具：
### 1-道具种类
不同道具对应不同操作，详见后文。
1、拨浪鼓
2、游戏机
3、钱
4、猫条
### 2-道具状态
道具只有两种状态，被持有在摄影师手上时和不被持有在摄影师手上时。道具除非被摄影师持有，否则不可沿z轴移动。

# 操作的对应关系

## 摄影师通过道具和操作对合影人员的交互

ex. 合影人员类型-其他状态名称-对应交互方法和道具

1、瞌睡虫-瞌睡-右手空手时使用e键交互键，瞌睡虫被一巴掌扇醒

2、社畜-掏出笔记本加班-右手空手时使用e键交互键，电脑被打飞

3、婴儿-大哭-对婴儿使用（道具）拨浪鼓
 
4、街机小子-发牢骚-对街机小子展示（道具）游戏机

5、母亲-着急-只要婴儿不哭即可

6、父亲-张望-对父亲展示（道具）钱

7、猫-乱动并且开始大叫-对猫使用（道具）猫条

## 合影人员间的相互影响

1、猫的叫声会增加婴儿大哭的概率

2、婴儿大哭会急剧增加母亲着急的概率

## 合影人员对摄影师的交互

1、当街机小子处于发牢骚的状态时，有概率会将处于其附近的摄影师手上的道具拍飞。
# 游戏进程
## 初始状态的生成

游戏开始时随机生成合影人员（有的位置可以是空）和类型，并生成对应的道具，随机分布在空地上。

## 刷新的控制
对人物状态的刷新按照固定帧率，暂定120bpm的刷新频率，并且让刷新频率和背景音乐的节奏一致。
# 游戏目标

## 游戏的结束

在整个游戏的倒计时（3：00）结束前，玩家必须为合影进行拍摄。

摄影师操纵摄像机时必须来到摄像机处（即镜头外），操作拍摄键，进入3s的拍摄倒计时，在拍摄倒计时结束时拍摄并且生成对应照片，根据此时处于笑的状态的合影人员数量计算分数。在拍摄倒计时倒计的过程中依然可以随时离开摄像机，则该次拍摄作废，不会导致游戏结束也不会影响分数。拍摄完成即给1000分，每有一位人员处于微笑状态根据不同类型增加100-500分不等。

## 交互的计分

每当摄影师成功将一位合影人员转变为笑的状态时，可获得额外的小额微笑加分，10分。只要E、J、L键，即交互键和道具键连续成功使用，即可让上述小额微笑加分获得连击加成，连击三次分数x1.5，四次x2.0，以此类推。

# 美术要求

## 美术与动画风格
参考节奏天国，整体线条和色彩都简约，关键要素要突出，使得能尽可能一眼看出人员的种类和要对其使用的对应道具。

动画只制作关键帧，动作夸张而有力，突显其冷幽默的特性。

人物和道具需要凸显，使用描边线或者将其做成纸板的样子等。
## UI
UI整体简约，尽可能设计得贴近老式照相馆的要素，如摄影师进行拍摄时，将老式拍摄机的拉绳的图标从屏幕右上向下出现，作为拍摄键（可进行拍摄）的提示。

当合影人员处于其他状态时，要在其身旁出现对应操作和道具的提示图标。

被持有的道具要在ui（物品栏）中显示其对应的图标。

## 贴图

1、所有角色 
角色图像的视角均为平视。

2、所有道具 
道具图像的视角尽量为俯视，突出其立体性。

3、背景和前景（摄像机）

4、按键提示（即按键E、J、Space等的英文字样，效果参考粗线条高对比高饱和的美漫风格，有较强视角冲击力和张力，可参考Persona5的按键提示）

## 动画的设计

摄影师的移动仅需两帧或者至多四帧，风格参考夸张滑稽的黑白线条漫画，如老夫子，史努比和三毛历险记等，帧的切换风格依然参考节奏天国，凸显出变化的运动性来彰显滑稽感。

合影人员处于微笑状态时，表情和动作基本不变，让其有个稍稍摇晃的状态即可，一样仅需较少的帧数。

对于不同的合影人员，展示其非笑的状态（包括从笑的状态转变为非笑状态的过程）的动画，大致做2-6帧，整体风格仍然参考节奏天国，尽可能地有力和夸张，并且能很好地彰显出合影人员此刻的状态（比如要让玩家能轻松地看出母亲的着急，并且这种着急和其怀中大哭的婴儿相关）。

对于摄影师对合影人员的交互的动画，大致要求依然如上，下面给出含有细节的展现方式的示例作为参考：

ex. 对街机小子展示游戏机：摄影师递出游戏机（1帧），街机小子拿着游戏机（1帧），游戏机的上方出现Victory！的字样（1帧），街机小子两眼放光咧嘴大笑（1帧）。

此外，摄影师对合影人员的交互的动画在播放上要卡上刷新频率，也即BGM的节奏；所以该动画长度也必须是节拍的整倍数（即上述120bpm的速率，一拍为0.5s）。

# 可能可以做的待定事项

1、可以通过和合影人员的互动获得额外的道具，如摄影师拍飞社畜的笔记本电脑时，可以获得一个额外的笔记本电脑道具。

2、如若玩家的交互操作落在节拍上，可以获得额外的分数奖励甚至是时间奖励。

