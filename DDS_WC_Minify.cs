/*
 * ++++++++     Diamond Dome Defense with Weaponcore    ++++++++
 * [DDS version: 5.6][keleios version: WC 0.1][Anu's Version: 0.3]
 * 
 * 
 * ************************************************************************************************
 * Setup Informations:
 * + To Display Status off DDS In Name off PB add | After Programable block name
 * 
 * + To Display infos on LCD add tag [DDS] to lcd and enter desiert parameters into CustomData
 * 
 * ddsversion
 * wcversion
 * pdc
 * designator
 * cameras
 * missiles
 * torpedos
 * targets
 * 
 * 
 */
const string Ѐ="5.6(WC 0.1)";const string Ͽ="Diamond Dome";public const string Ś="DDS";const string Ͼ="AGM";const string
Ͻ="DDS Designator";const string ϼ="DDS Camera";const string ϻ="DDS Aiming";const string Ϻ="DDS Door";const string Ϲ=
"DDS Display";const string ϸ="[DDS]";const string Ϸ="DDS Turret";const string ϵ="Azimuth";const string ϴ="Elevation";const string ϳ=
"Aiming";const string ϲ="Reload";const string ϱ="Forward";const string ϰ="Status";const string ϯ="Alert";const string Ϯ="AIM";
const string Ё="DDS Missile";const string ϭ="DDS Torpedo";const string Ђ="[CGE]";const string И="FIRE";const string Ж=
"LARGEST";const string Е="BIG";const string Д="EXTEND";const string Г="TRACK";const string В="RELEASE";const string Б="SET";const
string А="CENTER";const string Џ="OFFSET";const string Ў="RANDOM";const string З="RANGE";const string Ѝ="INCRANGE";const
string Ћ="DECRANGE";const string Њ="CYCLEOFFSET";const string Љ="TOGGLE";const string Ј="ENABLE";const string Ї="DISABLE";
const string І="AUTOLAUNCH";const string Ѕ="AUTOLAUNCH_ON";const string Є="AUTOLAUNCH_OFF";const string Ѓ="DEBUGMODE";public
const double Ř=0.707;const int Ќ=7;public const int ň=30;const int Ϡ=240;public const int ř=120;const double ϔ=0.75;const int
ϓ=60;const double ϒ=0.005;const int ϑ=(int)(1/ϒ);public const double ś=1.0/60.0;public const int ŝ=60;const UpdateType ϐ=
UpdateType.Terminal|UpdateType.Trigger|UpdateType.Script;const string Ϗ="IGCMSG_TK_IF";const string ώ="IGCMSG_TR_TK";const string
ύ="IGCMSG_TR_DL";const string ό="IGCMSG_TR_SW";const string ϋ="CGE_MSG_TR_DL";const string ϊ="SIMJ";const float ω=0.032f;
Color ψ=Color.White;Color χ=new Color(40,5,100);Color φ=new Color(245,230,255);Color υ=new Color(40,15,5);Color ϕ=Color.White
;Color τ=new Color(0,0,0);Color ϗ=new Color(50,50,50);Color Ϭ=new Color(255,255,255);Color Ϫ=new Color(100,100,0);Color ϩ
=Color.White;Color Ϩ=Color.Green;Color ϧ=Color.White;Color Ϧ=Color.DarkOrchid;Color ϥ=Color.White;Color Ϥ=new Color(0,0,
90);char[]ϣ=new char[]{':'};IComparer<Ū>Ϣ=new ſ();Random ϫ=new Random();List<IMyTerminalBlock>ϡ=new List<IMyTerminalBlock>
(0);StringBuilder á=new StringBuilder();StringBuilder ϟ=new StringBuilder();Dictionary<MyDetectedEntityInfo,float>Ϟ=new
Dictionary<MyDetectedEntityInfo,float>();int ϝ=0;Ӭ Ň;ѱ Ϝ;IMyRemoteControl ϛ;List<Ż>Ϛ;ɞ<Ż>ϙ;ɞ<Ż>ϖ;List<IMyTextSurface>Ϙ;int Й=1;int
ѕ=0;int ѓ=0;List<Ҙ>ђ;ɞ<Ҙ>ё;ɞ<Ҙ>ѐ;int я=-10000;List<IMyCameraBlock>ю;ÿ э;int ь;ʰ ы;յ є;SortedDictionary<double,Ū>ъ;double
ш=0;int ч;bool ц=false;int х=0;List<IMyProgrammableBlock>ф;List<IMyProgrammableBlock>у;List<ӏ>т;Dictionary<string,ӏ>с;
byte[]р=new byte[8];long[]щ=new long[1];int і=0;չ ў;IEnumerator<int>ѩ;IMyBroadcastListener ѧ;IMyBroadcastListener Ѧ;Queue<Ӛ>
ѥ;Queue<Ū>Ѥ;int ѣ=0;bool Ѣ=false;double ѡ=100;double ˇ=0;int Ѡ=0;int Ѩ=0;long џ=0;bool ѝ=true;ĥ ќ;bool ћ;int s=0;int њ=0;
bool љ=false;Program(){Runtime.UpdateFrequency=UpdateFrequency.Update1;ќ=new ĥ(Runtime,ϑ,ϒ);Ň=new Ӭ();Ň.ӫ=this;Ň.Ӫ=new ʗ();
if(!Ň.Ӫ.Ȁ(Me))Ň.Ӫ=null;Ϝ=new ѱ();}void Main(string ј,UpdateType ї){if(!љ){if(!Ч()){return;}ч=-100000;х=0;ѝ=true;ћ=false;ќ.
æ();s=-1;љ=true;return;}ќ.å();if(Ň.Ӫ==null){Ň.Ӫ=new ʗ();if(!Ň.Ӫ.Ȁ(Me))Ň.Ӫ=null;}if(ћ)ќ.à("InterGridComms");if(ј.Length>0)
{if(ј.Equals(ώ)){ζ();}else{if((ї&ϐ)!=0){ς(ј);}}}if(ћ)ќ.ß("InterGridComms");џ+=Runtime.TimeSinceLastRun.Ticks;if((ї&
UpdateType.Update1)==0||џ==0){return;}џ=0;s++;if(!ѝ){if(s%ϓ==0){Ӌ();}return;}if(ћ)ќ.à("AutoMissileLaunch");if(s>=і){μ();}if(ћ)ќ.ß(
"AutoMissileLaunch");if(ћ)ќ.à("MissileReload");if(Ň.ӑ>0&&s%Ň.ӑ==0){η();}if(ћ)ќ.ß("MissileReload");if(ћ)ќ.à("ManualAimReload");if(Ň.ӧ>0&&s%Ň
.ӧ==0){Ь();}if(ћ)ќ.ß("ManualAimReload");if(ћ)ќ.à("ManualRaycast");Ό();if(ћ)ќ.ß("ManualRaycast");if(ћ)ќ.à("Designator");ͽ(
);if(ћ)ќ.ß("Designator");if(ћ)ќ.à("Allies");ˑ();if(ћ)ќ.ß("Allies");if(ы.ʋ()>0){ц=true;if(ћ)ќ.à("RaycastTracking");if(s>=ь
){ʿ();}if(ћ)ќ.ß("RaycastTracking");if(ћ)ќ.à("AssignTargets");if(s%Ň.ӿ==0){ˏ();}if(ћ)ќ.ß("AssignTargets");if(ћ)ќ.à(
"PDCAimFireReload");θ();if(ћ)ќ.ß("PDCAimFireReload");}else if(ц){ц=false;foreach(Ż Ш in Ϛ){Ш.ļ=null;Ш.o();Ш.Ə();}х=0;}if(ћ)ќ.à(
"TransmitIGCMessages");if(ы.ʋ()>0||ѥ.Count()>0){γ();}έ();if(ћ)ќ.ß("TransmitIGCMessages");if(s%Ň.ҵ==0){Ի();Ϋ();}if(s%ϓ==0){Ӌ();}ќ.ä();}void Э(
){try{Ū д;IMyTerminalBlock Ы=(э.ú.Count>0?э.ú[0]:(IMyTerminalBlock)Me);MyDetectedEntityInfo Ъ=new MyDetectedEntityInfo(
9801,ϊ,MyDetectedEntityType.LargeGrid,Ы.WorldMatrix.Translation+(Ы.WorldMatrix.Forward*0.1),Ы.WorldMatrix,Vector3D.Zero,
MyRelationsBetweenPlayerAndBlock.Enemies,Ы.WorldAABB.Inflate(100),1);ы.ʳ(ref Ъ,1,out д);ζ();ς(ϊ);μ();ξ(д,false,true);Μ(у,д,Ӱ.ӯ,null,false,true);double Щ
=Ň.ӣ;Ň.ӣ=0.1f;т=new List<ӏ>();т.Add(new ӏ("AIM0",Me,null,null));Ό();Ň.ӣ=Щ;т=null;ʿ();ˏ();θ();ʿ();ˏ();θ();ы.ʝ();ы.ʟ();ц=
false;foreach(Ż Ш in Ϛ){Ш.ļ=null;Ш.o();Ш.Ə();}х=0;ѥ.Clear();Ѥ.Clear();γ();γ();ѥ.Clear();Ѥ.Clear();Ѣ=false;ѣ=0;}catch(
Exception){}}bool Ч(){if(њ==0){Н();List<IMyRemoteControl>Ц=new List<IMyRemoteControl>(0);GridTerminalSystem.GetBlocksOfType(Ц,(ȝ)
=>{if(ϛ==null){ϛ=ȝ;}return false;});if(ϛ!=null){float Х=ϛ.SpeedLimit;ϛ.SpeedLimit=float.MaxValue;ѡ=ϛ.SpeedLimit;ϛ.
SpeedLimit=Х;}if(Ň.ҳ){List<IMyTerminalBlock>Ф=new List<IMyTerminalBlock>();GridTerminalSystem.GetBlocksOfType(Ф,(ȝ)=>{return(ȝ is
IMyMechanicalConnectionBlock||ȝ is IMyShipConnector);});List<Ӓ>У=new List<Ӓ>();foreach(IMyTerminalBlock Ύ in Ф){if(Ύ is IMyMechanicalConnectionBlock
){IMyMechanicalConnectionBlock С=Ύ as IMyMechanicalConnectionBlock;if(С.Top!=null){У.Add(new Ӓ(С.Top.CubeGrid,С.Top.
Position));}}else{IMyShipConnector Р=Ύ as IMyShipConnector;if(Р!=null&&Р.OtherConnector!=null){У.Add(new Ӓ(Р.OtherConnector.
CubeGrid,Р.OtherConnector.Position));}}}if(Ň.Ҳ){շ П=new շ();ѩ=П.ב(new Ӓ(Me.CubeGrid,Me.Position),У,Ň.Ӆ,Ň.Ӈ);ў=П;њ=1;}else{Ү О=
new Ү(new Ӓ(Me.CubeGrid,Me.Position),У,Ň.Ӆ,Me);ў=О;њ=2;}}else{ў=null;}э=new ÿ(new List<IMyCameraBlock>(0));ы=new ʰ();є=new
յ();ъ=new SortedDictionary<double,Ū>();Ь();η();ѥ=new Queue<Ӛ>();Ѥ=new Queue<Ū>();Ň.ӣ=Math.Min(Math.Max(Ň.ӣ,1000),100000);
ѧ=IGC.RegisterBroadcastListener(ώ);Ѧ=IGC.RegisterBroadcastListener(Ϗ);ˇ=Me.CubeGrid.WorldAABB.HalfExtents.Length();Э();}
if(њ==1){if(ѩ.MoveNext()){Echo("--- Creating Occlusion Checker ---\nBlocks Processed:"+ѩ.Current);return false;}else{Echo(
"--- Occlusion Checker Created ---");ѩ.Dispose();ѩ=null;њ=2;}}return true;}void Н(){int М=Me.CustomData.GetHashCode();if(ϝ==0||ϝ!=М){ϝ=М;MyIni Л=new MyIni(
);if(Л.TryParse(Me.CustomData)){if(Л.ContainsSection(Ś)){Ň.ӧ=Л.Get(Ś,"MainBlocksReloadTicks").ToInt32(Ň.ӧ);Ň.ӥ=Л.Get(Ś,
"TargetTracksTransmitIntervalTicks").ToInt32(Ň.ӥ);Ň.Ӥ=Л.Get(Ś,"ManualAimBroadcastDurationTicks").ToInt32(Ň.Ӥ);Ň.ӣ=Л.Get(Ś,"ManualAimRaycastDistance").
ToDouble(Ň.ӣ);Ň.Ӣ=Л.Get(Ś,"ManualAimRaycastRefreshInterval").ToInt32(Ň.Ӣ);Ň.ӡ=Л.Get(Ś,"MaxDesignatorUpdatesPerTick").ToInt32(Ň.ӡ
);Ň.Ӡ=Л.Get(Ś,"MaxPDCUpdatesPerTick").ToSingle(Ň.Ӡ);Ň.ӟ=Л.Get(Ś,"MinPDCRefreshRate").ToInt32(Ň.ӟ);Ň.Ӟ=Л.Get(Ś,
"UseDesignatorReset").ToBoolean(Ň.Ӟ);Ň.ӝ=Л.Get(Ś,"DesignatorResetInterval").ToInt32(Ň.ӝ);Ň.Ӧ=Л.Get(Ś,"UseRangeSweeper").ToBoolean(Ň.Ӧ);Ň.ӽ=Л
.Get(Ś,"RangeSweeperPerTick").ToInt32(Ň.ӽ);Ň.Ӳ=Л.Get(Ś,"RangeSweeperInterval").ToInt32(Ň.Ӳ);Ň.ԉ=Л.Get(Ś,
"TargetFoundHoldTicks").ToInt32(Ň.ԉ);Ň.ӱ=Л.Get(Ś,"DisplayStatusInName").ToBoolean(Ň.ӱ);Ň.ԇ=Л.Get(Ś,"UsePDCSpray").ToBoolean(Ň.ԇ);Ň.Ԇ=Л.Get(Ś,
"PDCSprayMinTargetSize").ToDouble(Ň.Ԇ);Ň.Ԅ=Л.Get(Ś,"MaxRaycastTrackingDistance").ToDouble(Ň.Ԅ);Ň.ԃ=Л.Get(Ś,"RaycastTargetRefreshTicks").ToInt32
(Ň.ԃ);Ň.Ԃ=Math.Max(Л.Get(Ś,"RaycastGlobalRefreshTicks").ToInt32(Ň.Ԃ),1);Ň.ԁ=Л.Get(Ś,"PriorityMinRefreshTicks").ToInt32(Ň.
ԁ);Ň.Ԁ=Л.Get(Ś,"PriorityMaxRefreshTicks").ToInt32(Ň.Ԁ);Ň.ӿ=Math.Max(Л.Get(Ś,"priorityGlobalRefreshTicks").ToInt32(Ň.ӿ),1)
;Ň.Ԉ=Л.Get(Ś,"TargetSlippedTicks").ToInt32(Ň.Ԉ);Ň.Ӿ=Л.Get(Ś,"TargetLostTicks").ToInt32(Ň.Ӿ);Ň.ԅ=Л.Get(Ś,
"RandomOffsetProbeInterval").ToInt32(Ň.ԅ);Ň.Ӽ=Л.Get(Ś,"RaycastExtensionDistance").ToDouble(Ň.Ӽ);Ň.ӻ=Л.Get(Ś,"MinTargetSizeEngage").ToDouble(Ň.ӻ);Ň.
Ӻ=Л.Get(Ś,"MinTargetSizePriority").ToDouble(Ň.Ӻ);Ň.ӹ=Л.Get(Ś,"AutoMissileLaunch").ToBoolean(Ň.ӹ);Ň.Ӹ=Л.Get(Ś,
"MissileMinTargetSize").ToDouble(Ň.Ӹ);Ň.ӷ=Л.Get(Ś,"MissileCountPerSize").ToDouble(Ň.ӷ);Ň.Ӷ=Л.Get(Ś,"MaxMissileLaunchDistance").ToDouble(Ň.Ӷ);Ň
.ӵ=Л.Get(Ś,"MissileOffsetRadiusFactor").ToDouble(Ň.ӵ);Ň.Ӵ=Л.Get(Ś,"MissileOffsetProbability").ToDouble(Ň.Ӵ);Ň.ӛ=Л.Get(Ś,
"MissileStaggerWaitTicks").ToInt32(Ň.ӛ);Ň.ӆ=Л.Get(Ś,"MissileReassignIntervalTicks").ToInt32(Ň.ӆ);Ň.ӑ=Л.Get(Ś,"MissilePBGridReloadTicks").ToInt32(
Ň.ӑ);Ň.ӄ=Л.Get(Ś,"MissileTransmitDurationTicks").ToInt32(Ň.ӄ);Ň.Ӄ=Л.Get(Ś,"MissileTransmitIntervalTicks").ToInt32(Ň.Ӄ);Ň.
ӂ=Л.Get(Ś,"MissileLaunchSpeedLimit").ToDouble(Ň.ӂ);Ň.Ӂ=Л.Get(Ś,"PDCFireDotLimit").ToDouble(Ň.Ӂ);Ň.Ӏ=Л.Get(Ś,
"ConstantFireMode").ToBoolean(Ň.Ӏ);Ň.ҿ=Л.Get(Ś,"RotorUseLimitSnap").ToBoolean(Ň.ҿ);Ň.Ҿ=Л.Get(Ś,"RotorCtrlDeltaGain").ToSingle(Ň.Ҿ);Ň.ҹ=Л.
Get(Ś,"ReloadCheckTicks").ToInt32(Ň.ҹ);Ň.Ҹ=Л.Get(Ś,"ReloadedCooldownTicks").ToInt32(Ň.Ҹ);Ň.ҷ=Л.Get(Ś,"ReloadMaxAngle").
ToDouble(Ň.ҷ);Ň.Ҷ=Л.Get(Ś,"ReloadLockStateAngle").ToDouble(Ň.Ҷ);Ň.ҵ=Math.Max(Л.Get(Ś,"DisplaysRefreshInterval").ToInt32(Ň.ҵ),1);
Ň.Ҵ=Л.Get(Ś,"AllyTrackLostTicks").ToInt32(Ň.Ҵ);Ň.ҳ=Л.Get(Ś,"CheckSelfOcclusion").ToBoolean(Ň.ҳ);Ň.Ҳ=Л.Get(Ś,
"UseAABBOcclusionChecker").ToBoolean(Ň.Ҳ);Ň.Ӆ=Л.Get(Ś,"OcclusionExtraClearance").ToSingle(Ň.Ӆ);Ň.ұ=Л.Get(Ś,"UseFourPointOcclusion").ToBoolean(Ň.ұ
);Ň.Ӈ=Л.Get(Ś,"OcclusionCheckerInitBlockLimit").ToInt32(Ň.Ӈ);}}}}void Ь(){К();л();й();Ͱ();ΐ();Ώ();}void К(){List<
IMyLargeTurretBase>Ю;ռ(out Ю,Ͻ);if(Ю==null){Ю=new List<IMyLargeTurretBase>(0);}List<Ҙ>о=new List<Ҙ>(Ю.Count);foreach(IMyLargeTurretBase м
in Ю){о.Add(new Ҙ(м));}ђ=о;ё=new ɞ<Ҙ>(о,ԡ);ѐ=new ɞ<Ҙ>(о,Ԡ);}void л(){List<IMyCameraBlock>к;ռ(out к,ϼ,(ȝ)=>{ȝ.EnableRaycast
=true;ȝ.Enabled=true;return true;});if(к==null){к=new List<IMyCameraBlock>(0);}ю=к;э.ú=ю;}void й(){Dictionary<long,Ż>и=
null;{if(Ϛ!=null&&Ϛ.Count>0){и=new Dictionary<long,Ż>();foreach(Ż ˎ in Ϛ){if(!и.ContainsKey(ˎ.Ÿ.EntityId)){и.Add(ˎ.Ÿ.
EntityId,ˎ);}}}}HashSet<IMyTerminalBlock>з=new HashSet<IMyTerminalBlock>();List<IMyMotorStator>ж=new List<IMyMotorStator>();
Dictionary<long,List<IMyMotorStator>>н=new Dictionary<long,List<IMyMotorStator>>();Dictionary<long,List<IMyUserControllableGun>>е=
new Dictionary<long,List<IMyUserControllableGun>>();Dictionary<long,IMyTerminalBlock>г=new Dictionary<long,IMyTerminalBlock
>();Dictionary<long,IMyShipController>в=new Dictionary<long,IMyShipController>();Dictionary<long,IMyShipConnector>б=new
Dictionary<long,IMyShipConnector>();List<IMyBlockGroup>ˤ=new List<IMyBlockGroup>();List<Ż>а=new List<Ż>();GridTerminalSystem.
GetBlockGroups(ˤ,(ȝ)=>{return ȝ.Name.IndexOf(Ϸ,StringComparison.OrdinalIgnoreCase)>-1;});foreach(IMyBlockGroup ɼ in ˤ){ɼ.
GetBlocksOfType(ϡ,(Ύ)=>{if(Me.IsSameConstructAs(Ύ)&&з.Add(Ύ)){if(Ύ is IMyMotorStator){if(Ԟ(Ύ,ϵ)){ж.Add(Ύ as IMyMotorStator);}else if(Ԟ(
Ύ,ϴ)){if(н.ContainsKey(Ύ.CubeGrid.EntityId)){н[Ύ.CubeGrid.EntityId].Add(Ύ as IMyMotorStator);}else{List<IMyMotorStator>Я=
new List<IMyMotorStator>();Я.Add(Ύ as IMyMotorStator);н.Add(Ύ.CubeGrid.EntityId,Я);}}}else if(Ύ is IMyUserControllableGun){
if(е.ContainsKey(Ύ.CubeGrid.EntityId)){е[Ύ.CubeGrid.EntityId].Add(Ύ as IMyUserControllableGun);}else{List<
IMyUserControllableGun>Я=new List<IMyUserControllableGun>();Я.Add(Ύ as IMyUserControllableGun);е.Add(Ύ.CubeGrid.EntityId,Я);}}else if(Ύ is
IMyShipController){if(!в.ContainsKey(Ύ.CubeGrid.EntityId)){в.Add(Ύ.CubeGrid.EntityId,Ύ as IMyShipController);}}else if(Ύ is
IMyShipConnector){if(!б.ContainsKey(Ύ.CubeGrid.EntityId)){if(Ԟ(Ύ,ϲ)){б.Add(Ύ.CubeGrid.EntityId,Ύ as IMyShipConnector);}}}else if(Ԟ(Ύ,ϳ))
{if(!г.ContainsKey(Ύ.CubeGrid.EntityId)){г.Add(Ύ.CubeGrid.EntityId,Ύ);}}}return false;});}foreach(IMyMotorStator Ŏ in ж){
if(Ŏ.TopGrid!=null){List<IMyMotorStator>ͻ;if(н.TryGetValue(Ŏ.TopGrid.EntityId,out ͻ)){List<IMyMotorStator>Ñ=new List<
IMyMotorStator>();List<List<IMyUserControllableGun>>ŋ=new List<List<IMyUserControllableGun>>();List<IMyTerminalBlock>ƶ=new List<
IMyTerminalBlock>();Ȭ ͺ=null;IMyTerminalBlock ͷ=null;foreach(IMyMotorStator U in ͻ){if(U.TopGrid!=null){List<IMyUserControllableGun>Ͷ;if
(е.TryGetValue(U.TopGrid.EntityId,out Ͷ)){IMyTerminalBlock ō;if(г.ContainsKey(U.TopGrid.EntityId)){ō=г[U.TopGrid.EntityId
];}else{ō=Ͷ[0];}Ñ.Add(U);ŋ.Add(Ͷ);ƶ.Add(ō);if(ͺ==null&&Ͷ.Count>0){ͺ=Ԣ(Ͷ[0]);}if(ͷ==null){ͷ=ō;}}}}if(Ñ.Count>0){
IMyShipController Ō;if(в.ContainsKey(Ñ[0].TopGrid.EntityId)){Ō=в[Ñ[0].TopGrid.EntityId];}else{Ō=ϛ;}if(ͺ==null){ͺ=Ϝ.Ґ;}Ż ˎ=new Ż(Ŏ.
CustomName,Ŏ,Ñ,ƶ,Ō,ŋ,ͺ,Ň);IMyShipConnector ʹ=null;if(б.ContainsKey(Ñ[0].CubeGrid.EntityId)){ʹ=б[Ñ[0].CubeGrid.EntityId];}
IMyShipConnector ͳ=null;if(б.ContainsKey(ͷ.CubeGrid.EntityId)){ͳ=б[ͷ.CubeGrid.EntityId];}if(ʹ!=null&&ͳ!=null){ˎ.ĺ=ʹ;ˎ.Ĺ=ͳ;}if(и!=null&&и
.ContainsKey(ˎ.Ÿ.EntityId)){ˎ.ǂ(и[ˎ.Ÿ.EntityId]);}else{ˎ.o(true);ˎ.Ə();}а.Add(ˎ);}}}}if(Ň.Ӡ==0){Й=Math.Max((int)Math.
Ceiling(Ň.ӟ*а.Count/(double)ŝ),1);ѕ=0;}else{if(Ň.Ӡ<1f&&Ň.Ӡ>0){Й=1;ѕ=Math.Min((int)Math.Floor(1f/Ň.Ӡ),ŝ);}else{Й=Math.Max(Math.
Min(а.Count,(int)Math.Ceiling(Ň.Ӡ)),1);ѕ=0;}}double Ͳ=(double)а.Count/Й;double ͱ=Ͳ/Math.Max(Ň.Ԁ,1);foreach(Ż ˎ in а){ˎ.Ľ=ͱ;
ˎ.Ĵ=MathHelperD.ToRadians(Ň.ҷ);ˎ.ĳ=MathHelperD.ToRadians(Ň.Ҷ);ˎ.ľ=ў;}Ϛ=а;ϙ=new ɞ<Ż>(а,ԟ);ϖ=new ɞ<Ż>(а,ԟ);}void Ͱ(){List<ӏ
>ˮ=new List<ӏ>();Dictionary<string,ӏ>ˬ=new Dictionary<string,ӏ>();List<IMyBlockGroup>ˤ=new List<IMyBlockGroup>();
GridTerminalSystem.GetBlockGroups(ˤ,(ȝ)=>{return Ԟ(ȝ,ϻ);});foreach(IMyBlockGroup ɼ in ˤ){ɼ.GetBlocksOfType(ϡ,(ȝ)=>{int ű=1;int Ε=ȝ.
CustomName.IndexOf(Ϯ,StringComparison.OrdinalIgnoreCase);if(Ε>-1){if(Ε+ϱ.Length<ȝ.CustomName.Length){if(int.TryParse(
$"{ȝ.CustomName[Ε+Ϯ.Length]}",out ű)){if(ű<1||ű>9){ű=1;}}else{ű=1;}}}string Δ=Ϯ+ű;ӏ Α;if(ˬ.ContainsKey(Δ)){Α=ˬ[Δ];}else{if(с!=null&&с.ContainsKey(Δ))
{Α=с[Δ];}else{Α=new ӏ(Δ,null,null,null);Α.ò=Ň.ӣ;}ˮ.Add(Α);ˬ.Add(Δ,Α);}if(Ԟ(ȝ,ϱ)){Α.Ӎ=ȝ;Α.ӌ=ȝ as IMyLargeTurretBase;}else
if(Ԟ(ȝ,ϯ)){Α.ӊ=ȝ;}else if(Ԟ(ȝ,ϰ)){IMyTextSurfaceProvider Γ=ȝ as IMyTextSurfaceProvider;if(Γ!=null){try{Α.Ӌ=Γ.GetSurface(0)
;}catch(Exception){}}}else{if(Α.Ӎ==null){Α.Ӎ=ȝ;Α.ӌ=ȝ as IMyLargeTurretBase;}if(Α.Ӌ==null&&ȝ is IMyTextSurfaceProvider){
try{Α.Ӌ=((IMyTextSurfaceProvider)ȝ).GetSurface(0);}catch(Exception){}}}return false;});break;}int Β=0;while(Β<ˮ.Count){ӏ Α=
ˮ[Β];if(Α.Ӎ==null){if(Β+1==ˮ.Count){ˮ.RemoveAt(Β);}else{ˮ[Β]=ˮ[ˮ.Count-1];ˮ.RemoveAt(ˮ.Count-1);}ˬ.Remove(Α.ӎ);}Β++;}т=ˮ;
с=ˬ;}void ΐ(){ռ(out Ϙ,Ϲ);}void Ώ(){GridTerminalSystem.GetBlocksOfType(Զ,Ύ=>Ύ.IsSameConstructAs(Me)&&Ύ.CustomName.Contains
(ϸ));}void Ό(){if(т?.Count>0&&Ň.Ӣ>0&&(s%Ň.Ӣ<т.Count)){ӏ Α=т[s%Ň.Ӣ];if(ԥ(Α.Ӎ)&&Α.Ӊ){Vector3D Î=Α.ŭ();Vector3D ƥ=Α.Ӎ.
WorldMatrix.Translation+(Î*Α.ò);MyDetectedEntityInfo ɶ;э.ɷ(ref ƥ,out ɶ,Ň.Ӽ);if(!ɶ.IsEmpty()&&Ԥ(ref ɶ)){Α.ӈ=ɶ.EntityId;Α.Ũ=(ɶ.
HitPosition.HasValue?ɶ.HitPosition.Value:ɶ.Position);Α.ţ=ɶ.Velocity;Α.ŧ=s;Vector3D Ή=Α.Ũ-Α.Ӎ.WorldMatrix.Translation;Ή=Vector3D.
ProjectOnVector(ref Ή,ref Î)+Α.Ӎ.WorldMatrix.Translation;Α.ʧ=Vector3D.TransformNormal(Ή-ɶ.Position,MatrixD.Transpose(ɶ.Orientation));Ū
Á;ы.ʳ(ref ɶ,s,out Á);if(Á!=null){Α.Ӊ=false;if(ԥ(Α.ӊ)){if(Α.ӊ is IMySoundBlock){((IMySoundBlock)Α.ӊ).Play();}else if(Α.ӊ
is IMyTimerBlock){((IMyTimerBlock)Α.ӊ).Trigger();}else{IMyFunctionalBlock Έ=Α.ӊ as IMyFunctionalBlock;if(Έ!=null&&!Έ.
Enabled){Έ.Enabled=true;}}}Á.ƅ=Α.ƅ;Á.Ƅ=Α.Ƅ;foreach(Ӛ Ά in ѥ){if(Ά.ʙ==Α){Ά.ʙ=Á;Ά.ӗ=s+Ň.ӄ;}}}}}else if(Α.ӈ>0&&!ы.ʏ(Α.ӈ)){Α.ӈ=-1;}
}}void ͽ(){var ͼ=Me.CubeGrid.EntityId;if(Ň.Ӫ!=null){Ϟ.Clear();Ň.Ӫ.ǎ(Me,Ϟ);foreach(var Á in Ϟ.Keys){MyDetectedEntityInfo ɶ
=Á;if(ы.ʳ(ref ɶ,s)){ч=s+Ň.ԁ;}}}else{ё.ʬ();int Ί=(Ň.ӡ==0?ђ.Count:Ň.ӡ);for(int Ǝ=0;Ǝ<Ί;Ǝ++){Ҙ ˀ=ё.ʫ();if(ˀ!=null){
MyDetectedEntityInfo ɶ=ˀ.Ҏ.GetTargetedEntity();if(ы.ʳ(ref ɶ,s)){ч=s+Ň.ԁ;}я=s;if(Ň.Ӟ){if(s>=ˀ.ҋ){ˀ.Ҏ.ResetTargetingToDefault();ˀ.Ҏ.
EnableIdleRotation=false;ˀ.ҋ=s+Ň.ӝ;}}}else{break;}}if(Ň.Ӧ){if(s<=я+Ň.ԉ){ѐ.ʬ();Ί=(Ň.ӽ==0?ђ.Count:Ň.ӽ);for(int Ǝ=0;Ǝ<Ί;Ǝ++){Ҙ ˀ=ѐ.ʫ();if(ˀ!=
null){if(s>=ˀ.Ҋ){ˀ.ҁ();ˀ.Ҋ=s+Ň.Ӳ;}}else{break;}}}}}}void ʿ(){Ū Á=ы.ʆ();if(Á!=null){if(s-Á.Ť>=Ň.ԃ){if(э.ú.Count>0){double ʾ=(
Á.ƅ==0?Ň.Ԅ:Á.ƅ);if((Á.Ũ-Me.WorldMatrix.Translation).LengthSquared()<=ʾ*ʾ){Vector3D ƥ=Á.Ũ+(Á.ţ*(s-Á.ŧ)*ś);
MyDetectedEntityInfo ɶ;if(э.ɷ(ref ƥ,out ɶ,Ň.Ӽ)){ʺ(ref ɶ);}ь=s+Ň.Ԃ;}}ы.ʐ(Á.ũ,s);ˢ(Á);}}if(Ň.ԇ&&Ň.ԅ>0&&s%Ň.ԅ==0){Ū ʡ=ы.ʒ();if(ʡ!=null&&ʡ.Ų!=
null&&ʡ.Ɖ>=Ň.Ԇ*Ň.Ԇ){double ʾ=(ʡ.ƅ==0?Ň.Ԅ:ʡ.ƅ);Vector3D ʽ=ʡ.Ŧ+(ʡ.ţ*(s-ʡ.ŧ)*ś);if((ʽ-Me.WorldMatrix.Translation).LengthSquared
()<=ʾ*ʾ){double ʼ=Math.Sqrt(ʡ.Ɖ)*0.5*ϔ;Vector3D ʻ=new Vector3D(((ϫ.NextDouble()*2)-1)*ʼ,((ϫ.NextDouble()*2)-1)*ʼ,((ϫ.
NextDouble()*2)-1)*ʼ);Vector3D ƥ=ʽ+Vector3D.TransformNormal(ʻ,ʡ.Ų.Value);MyDetectedEntityInfo ɶ;if(э.ɷ(ref ƥ,out ɶ,Ň.Ӽ)){ʺ(ref ɶ);
}ь=s+Ň.Ԃ;}}}}void ʺ(ref MyDetectedEntityInfo ɶ){if(ɶ.IsEmpty()){return;}if(Ԥ(ref ɶ)){Ū Á;ы.ʳ(ref ɶ,s,out Á);if(Á==null){
return;}Á.Ɖ=ɶ.BoundingBox.Extents.LengthSquared();if(Á.Ɔ==0){Á.Ɔ=Á.Ɖ;}if(Ň.ԇ&&Á.Ɖ>=Ň.Ԇ*Ň.Ԇ&&ɶ.HitPosition.HasValue){if(Á.ƀ==
null){Á.ƀ=new List<ʧ>(Ќ);}Vector3D ʹ=Vector3D.TransformNormal(ɶ.HitPosition.Value-ɶ.Position,MatrixD.Transpose(ɶ.Orientation
));if(Á.ƀ.Count>=Ќ){int ˁ=0;double ʸ=double.MaxValue;for(int Ǝ=0;Ǝ<Á.ƀ.Count;Ǝ++){if(s>Á.ƀ[Ǝ].ʥ+Ϡ){ˁ=Ǝ;ʸ=0;break;}double
ʁ=(Á.ƀ[Ǝ].ʦ-ʹ).LengthSquared();if(ʁ<ʸ){ˁ=Ǝ;ʸ=ʁ;}}if(ʸ<double.MaxValue){Á.ƀ[ˁ]=new ʧ(ref ʹ,s);}}else{Á.ƀ.Add(new ʧ(ref ʹ,s
));}}}else{ы.ʞ(ɶ.EntityId);ы.ʠ(ɶ.EntityId);}}void ˢ(Ū Á){Vector3D Ƨ=Me.GetPosition();Vector3D ˆ=(ϛ!=null?ϛ.
GetShipVelocities().LinearVelocity:Vector3D.Zero);if(Á.Ɖ>0&&Á.Ɖ<Ň.ӻ*Ň.ӻ){ы.ʞ(Á.ũ);ы.ʠ(Á.ũ);}else if(s-Á.ŧ<=Ň.Ӿ){double ˡ=ˈ(ˇ,Ƨ,ˆ,Á);ˡ+=ϫ.
NextDouble()*0.000000000001;Á.Ƈ=ˡ;ш=Math.Max(ˡ,ш);if(ϛ==null||ϛ.GetShipVelocities().LinearVelocity.LengthSquared()<=Ň.ӂ*Ň.ӂ){if(Á.
Ɖ>=Ň.Ӹ*Ň.Ӹ){if(Á.Ɓ==0||s>=Á.Ɓ+Ň.ӆ){Á.Ɓ=s;double ˠ=(Á.Ƅ==0?Ň.Ӷ:Á.Ƅ);if((Á.Ũ-Me.WorldMatrix.Translation).LengthSquared()<=ˠ
*ˠ){if(Á.Ɖ==0){Á.Ƃ=1;}else{Á.Ƃ=(int)Math.Ceiling(Math.Sqrt(Á.Ɖ)/Math.Max(Ň.ӷ,1));}}}}}}else{ы.ʠ(Á.ũ);}}void ˑ(){Қ ː=є.ҟ()
;if(ː!=null){if(s-ː.Š>Ň.Ҵ){є.Ҟ(ː.ũ);}}}void ˏ(){switch(х){case 0:if(s>=ч){if(ы.ʋ()>0){ч=s+Ň.Ԁ;ъ.Clear();ш=0;List<Ū>ˣ=ы.ʈ(
);foreach(Ū Á in ˣ){if(!ъ.ContainsKey(Á.Ƈ)){ъ.Add(Á.Ƈ,Á);ш=Math.Max(Á.Ƈ,ш);}}ϙ.Ħ();ϙ.ʬ();х=1;}}break;case 1:Ż ˎ=ϙ.ʫ();if(
ˎ!=null){if(ˎ!=null&&!ˎ.Ļ){double ˍ=0;Ū ˌ=null;double ˋ=0;Ū ˊ=null;foreach(KeyValuePair<double,Ū>ˉ in ъ){if(ˎ.Å(ˉ.Value,s
)){ˍ=ˉ.Key;ˌ=ˉ.Value;if(ˎ.Ń>0){if(ˌ.Ɖ>=ˎ.Ń*ˎ.Ń){ˋ=ˍ;ˊ=ˌ;break;}}else{break;}}}if(ˊ!=null){ˍ=ˋ;ˌ=ˊ;}if(ˌ!=null){ˎ.ļ=ˌ;if(ъ
.ContainsKey(ˍ)){ъ.Remove(ˍ);ш+=1000;ˍ=ш;ъ.Add(ˍ,ˌ);}}else{if(ˎ.ļ!=null&&!ы.ʏ(ˎ.ļ.ũ)){ˎ.ļ=null;}ˎ.o();ˎ.Ə();}}}else{х=0;}
break;}}double ˈ(double ˇ,Vector3D Ƨ,Vector3D ˆ,Ū Á){Vector3D Τ=Á.Ũ-Ƨ;double ˡ=Τ.Length();PlaneD λ;if(ˆ.LengthSquared()<0.01)
{λ=new PlaneD(Ƨ,Vector3D.Normalize(Τ));}else{λ=new PlaneD(Ƨ,Ƨ+ˆ,Ƨ+Τ.Cross(ˆ));}Vector3D κ=λ.Intersection(ref Á.Ũ,ref Á.ţ)
;Vector3D ι=κ-Á.Ũ;if(ι.Dot(ref Á.ţ)<0){ˡ+=Ň.ӳ*4;}else{double Ȧ=Math.Sqrt(ι.LengthSquared()/Math.Max(Á.ţ.LengthSquared(),
0.000000000000001));if((κ-(Ƨ+(ˆ*Ȧ))).LengthSquared()>ˇ*ˇ){ˡ+=Ň.ӳ*2;}else if(Á.Ɖ<=0){ˡ+=Ň.ӳ;}else if(Á.Ɖ<Ň.Ӻ*Ň.Ӻ){ˡ+=Ň.ӳ*3;}}if(Á.Ɔ>Á.Ɖ){ˡ
+=Ň.ӳ*Math.Max(Á.ƃ,1);}else{ˡ+=Ň.ӳ*Math.Min(Á.ƃ,1);}return ˡ;}void θ(){if(ѕ>0){if(s>=ѓ){ѓ=s+ѕ;}else{return;}}ϖ.ʬ();int Ί=(
Й==0?Ϛ.Count:Й);for(int Ǝ=0;Ǝ<Ί;Ǝ++){Ż ˎ=ϖ.ʫ();if(ˎ!=null){if(s>=ˎ.ķ+Ň.ҹ){ˎ.ķ=s;if(ˎ.Ç()){if(s>=ˎ.Ķ+Ň.Ҹ){ˎ.Ķ=s;ˎ.Æ(s);}}}
if(ˎ.ĵ==Ż.Ē.đ){if(ˎ.ļ!=null){ˎ.Â(ˎ.ļ,s);}}else{ˎ.Æ(s);}}else{break;}}}void η(){List<IMyBlockGroup>ˤ=new List<IMyBlockGroup
>();ф=new List<IMyProgrammableBlock>();GridTerminalSystem.GetBlockGroups(ˤ,(ȝ)=>{return Ԟ(ȝ,Ё);});foreach(IMyBlockGroup ɼ
in ˤ){ɼ.GetBlocksOfType(ф,(ȝ)=>{if(ȝ.Enabled&&Me.IsSameConstructAs(ȝ)){return լ.թ(ȝ);}else{return false;}});break;}ˤ.Clear
();у=new List<IMyProgrammableBlock>();GridTerminalSystem.GetBlockGroups(ˤ,(ȝ)=>{return Ԟ(ȝ,ϭ);});foreach(IMyBlockGroup ɼ
in ˤ){ɼ.GetBlocksOfType(у,(ȝ)=>{if(ȝ.Enabled&&Me.IsSameConstructAs(ȝ)){return լ.թ(ȝ);}else{return false;}});break;}}void ζ
(){while(ѧ.HasPendingMessage){object ε=ѧ.AcceptMessage().Data;if(ε is MyTuple<long,long,Vector3D,Vector3D,double>){
MyTuple<long,long,Vector3D,Vector3D,double>δ=(MyTuple<long,long,Vector3D,Vector3D,double>)ε;if(!ы.ʏ(δ.Item2)||s-ы.ʊ(δ.Item2).Š
>=Ň.Ԉ){ʣ ʲ=new ʣ();ʲ.ũ=δ.Item2;ʲ.Ũ=δ.Item3;ʲ.ţ=δ.Item4;ы.ʳ(ʲ,s-1,false);if(δ.Item5>0){Ū Á=ы.ʊ(ʲ.ũ);if(Á!=null&&Á.Ɖ==0){Á.Ɖ
=δ.Item5;}}}}}while(Ѧ.HasPendingMessage){object ε=Ѧ.AcceptMessage().Data;if(ε is MyTuple<long,long,Vector3D,Vector3D,
double>){MyTuple<long,Vector3D,Vector3D,double,int,long>ά=(MyTuple<long,Vector3D,Vector3D,double,int,long>)ε;if(!ы.ʏ(ά.Item1)
||s-ы.ʊ(ά.Item1).Š>=Ň.Ԉ){if((ά.Item5&(int)ѿ.Ұ)==0){ʣ ʲ=new ʣ();ʲ.ũ=ά.Item1;ʲ.Ũ=ά.Item2;ʲ.ţ=ά.Item3;ы.ʳ(ʲ,s-1,false);if(ά.
Item4>0){Ū Á=ы.ʊ(ʲ.ũ);if(Á!=null){if(Á.Ɖ==0){Á.Ɖ=ά.Item4;}Á.Ŵ=(ά.Item5&(int)ѿ.Ŵ)>0;}}}else{Қ ː=new Қ(ά.Item1);ː.Ũ=ά.Item2;ː.ţ
=ά.Item3;ː.Ң=ά.Item4;ː.Ŵ=(ά.Item5&(int)ѿ.Ŵ)>0;є.ղ(ː,s);}}}}}void ς(string ρ){string[]π=ρ.Split(ϣ,StringSplitOptions.
RemoveEmptyEntries);if(π.Length==0)return;string ο=π[0].Trim().ToUpper();ӏ Α=null;if(ο.StartsWith(Ϯ,StringComparison.OrdinalIgnoreCase)){
int ű;if(ο.Length==Ϯ.Length){ű=1;}else if(!int.TryParse(ο.Substring(Ϯ.Length).Trim(),out ű)){ű=0;}if(ű>=1&&с.ContainsKey(Ϯ+
ű)){Α=с[Ϯ+ű];}if(Α!=null&&π.Length<=1){return;}}if(Α!=null){ο=π[1].Trim().ToUpper();}else if(т?.Count>0){Α=т[0];}switch(ο
){case И:bool σ=վ(π,Е);List<IMyProgrammableBlock>Λ=(σ?у:ф);if(վ(π,Ж)){Ū ʡ=ы.ʒ();if(ʡ!=null){Μ(Λ,ʡ,Α?.ų??Ӱ.ӯ,Α?.ʧ,σ);}}
else if(Α!=null){bool μ=վ(π,Д);if(Α.ӈ>0&&ы.ʏ(Α.ӈ)){Ū ν=ы.ʊ(Α.ӈ);if(ν!=null){if(μ){ν.Ƅ=Math.Max(Α.ò,Ň.Ӷ);}Μ(Λ,ν,Α.ų,Α.ʧ,σ);}}
else{Α.Ӊ=true;Α.ӈ=-1;Α.ʧ=Vector3D.Zero;Α.Ũ=Α.Ӎ.WorldMatrix.Translation+(Α.ŭ()*Α.ò);Α.ƅ=Math.Max(Α.ò,Ň.Ԅ);Α.Ƅ=(μ?Math.Max(Α.ò
,Ň.Ӷ):0);Μ(Λ,Α,Α.ų,Α.ʧ,σ);}}break;case Г:if(Α!=null){if(Α.Ӊ&&Α.ӈ==-1){Α.Ӊ=false;Α.ʧ=Vector3D.Zero;}else{bool μ=վ(π,Д);Α.Ӊ
=true;Α.ӈ=-1;Α.ʧ=Vector3D.Zero;Α.Ũ=Α.Ӎ.WorldMatrix.Translation+(Α.ŭ()*Α.ò);Α.ƅ=Math.Max(Α.ò,Ň.Ԅ);Α.Ƅ=(μ?Math.Max(Α.ò,Ň.Ӷ)
:0);}}break;case В:if(Α!=null){Α.Ӊ=false;Α.ӈ=-1;Α.ʧ=Vector3D.Zero;}break;case Б:if(Α!=null&&π.Length>=3){switch(π[2].
ToUpper().Trim()){case А:Α.ų=Ӱ.ӯ;break;case Џ:Α.ų=Ӱ.Ӯ;break;case Ў:Α.ų=Ӱ.ӭ;break;case З:if(π.Length>=4){double E;if(double.
TryParse(π[3].Trim(),out E)){Α.ò=Math.Min(Math.Max(E,1000),100000);;}}break;}}break;case Њ:if(Α!=null){Α.ų=(Ӱ)(((int)Α.ų+1)%3);}
break;case Ѝ:if(Α!=null){Α.ò=Math.Min(Math.Max(Α.ò+1000,1000),100000);;}break;case Ћ:if(Α!=null){Α.ò=Math.Min(Math.Max(Α.ò-
1000,1000),100000);;}break;case Љ:ѝ=!ѝ;ӱ();if(ѝ==false){foreach(Ż ˎ in Ϛ){ˎ.ļ=null;ˎ.o(true);ˎ.Ə();}}break;case Ј:ѝ=true;ӱ()
;break;case Ї:ѝ=false;ӱ();foreach(Ż ˎ in Ϛ){ˎ.ļ=null;ˎ.o(true);ˎ.Ə();}break;case І:Ň.ӹ=!Ň.ӹ;break;case Ѕ:Ň.ӹ=true;break;
case Є:Ň.ӹ=false;break;case Ѓ:ћ=!ћ;break;}}void μ(){if(Ň.ӹ){if(ы.ʋ()>0){Ū Á=ы.ʆ();if(Á!=null){if(Á.Ƃ>0&&ы.ʏ(Á.ũ)){Á.Ƃ--;bool
ή=(Á.Ƃ<=0?true:(ϫ.NextDouble()<=Ň.Ӵ));ξ(Á,ή);і=s+Ň.ӛ;}}}}}void ξ(Ū Á,bool ή=false,bool Θ=false){IMyProgrammableBlock Σ=
null;double Ρ=double.MaxValue;foreach(IMyProgrammableBlock Ύ in ф){if(Ύ.IsWorking&&GridTerminalSystem.GetBlockWithId(Ύ.
EntityId)!=null){double Π=(Ύ.WorldMatrix.Translation-Á.Ũ).LengthSquared();if(Π<Ρ){Ρ=Π;Σ=Ύ;}}}if(Σ!=null&&!Θ){Ӛ Ά=new Ӛ();Ά.Ә=Բ()
;Ά.ʙ=Á;Ά.ӗ=s+Ň.ӄ;Ά.ӕ=ή;ѥ.Enqueue(Ά);Ӛ Ο=new Ӛ();Ο.ʙ=Á;Ο.ӗ=int.MaxValue;Ο.Ӕ=true;ѥ.Enqueue(Ο);MyTuple<long,long,long,int,
float>Ξ=new MyTuple<long,long,long,int,float>();Ξ.Item1=Me.EntityId;Ξ.Item2=Ά.Ә;Ξ.Item3=Me.EntityId;Ξ.Item4|=(int)Ҁ.ѽ;Ξ.Item5
=(ή?(float)(Math.Sqrt(Á.Ɖ)*0.5*Ň.ӵ):0f);bool Ν=IGC.SendUnicastMessage(Σ.EntityId,"",Ξ);if(!Ν){MyIni Ζ=new MyIni();if(Σ.
CustomData.Length>0)Ζ.TryParse(Σ.CustomData);Ζ.Set(Ͼ,"UniqueId",Ξ.Item2);Ζ.Set(Ͼ,"GroupId",Ξ.Item3);if(ή){Ζ.Set(Ͼ,
"OffsetTargeting",Ξ.Item4);Ζ.Set(Ͼ,"RandomOffsetAmount",Ξ.Item5);}Σ.CustomData=Ζ.ToString();Σ.TryRun("FIRE:"+Me.EntityId);}ф.Remove(Σ);}}
void Μ(List<IMyProgrammableBlock>Λ,Ū Á,Ӱ Κ=Ӱ.ӯ,Vector3D?ʹ=null,bool Ι=false,bool Θ=false){for(int Ǝ=0;Ǝ<Λ.Count;Ǝ++){
IMyProgrammableBlock Η=Λ[Ǝ];if(Η.IsWorking&&GridTerminalSystem.GetBlockWithId(Η.EntityId)!=null&&!Θ){Ӛ Ά=new Ӛ();Ά.Ә=Բ();Ά.ʙ=Á;Ά.ӗ=s+Ň.Ӥ;if(
Ι&&Ԟ(Η,Ђ))Ά.ӓ=Η;ѥ.Enqueue(Ά);MyIni Ζ=null;MyTuple<long,long,long,int,float>Ξ=new MyTuple<long,long,long,int,float>();Ξ.
Item1=Me.EntityId;Ξ.Item2=Ά.Ә;Ξ.Item3=Me.EntityId;switch(Κ){case Ӱ.Ӯ:Ξ.Item4|=(int)Ҁ.ѽ;Ζ=new MyIni();if(Η.CustomData.Length>0
)Ζ.TryParse(Η.CustomData);Ζ.Set(Ͼ,"ProbeOffsetVector",ԧ(ʹ!=null?ʹ.Value:Vector3D.Zero));Η.CustomData=Ζ.ToString();break;
case Ӱ.ӭ:Ξ.Item4|=(int)Ҁ.ѽ;Ξ.Item5=(float)(Math.Sqrt(Á.Ɖ)*0.5*Ň.ӵ);break;default:Ξ.Item5=0f;break;}bool Ν=IGC.
SendUnicastMessage(Η.EntityId,"",Ξ);if(!Ν){if(Ζ==null){Ζ=new MyIni();if(Η.CustomData.Length>0)Ζ.TryParse(Η.CustomData);}Ζ.Set(Ͼ,"UniqueId"
,Ξ.Item2);Ζ.Set(Ͼ,"GroupId",Ξ.Item3);switch(Κ){case Ӱ.Ӯ:Ζ.Set(Ͼ,"OffsetTargeting",Ξ.Item4);Ζ.Set(Ͼ,"ProbeOffsetVector",ԧ(
ʹ!=null?ʹ.Value:Vector3D.Zero));break;case Ӱ.ӭ:Ζ.Set(Ͼ,"OffsetTargeting",Ξ.Item4);Ζ.Set(Ͼ,"RandomOffsetAmount",Ξ.Item5);
break;}Η.CustomData=Ζ.ToString();Η.TryRun("FIRE:"+Me.EntityId);}if(Λ.Count==1){Λ.Clear();}else{Λ[Ǝ]=Λ[Λ.Count-1];Λ.RemoveAt(Λ
.Count-1);}break;}}}void γ(){Ѣ=false;if(ѣ==0){β();if(!Ѣ){ί();}ѣ=1;}else{ί();if(!Ѣ){β();}ѣ=0;}}void β(){while(ѥ.Count>0){Ӛ
ï=ѥ.Dequeue();if(s<=ï.ӗ){if(!(ï.ʙ.ũ==-1||ы.ʏ(ï.ʙ.ũ))){break;}if(s>=ï.Ӗ){if(ï.ӓ==null){MyTuple<bool,long,long,long,
Vector3D,Vector3D>α=new MyTuple<bool,long,long,long,Vector3D,Vector3D>();α.Item1=ï.Ӕ;α.Item2=(ï.Ӕ?Me.EntityId:ï.Ә);α.Item3=0;α.
Item4=ï.ʙ.ũ;α.Item5=ï.ʙ.Ũ+(ï.ʙ.ţ*(s-ï.ʙ.ŧ+1)*ś);α.Item6=ï.ʙ.ţ;IGC.SendBroadcastMessage(ï.Ӕ?ό:ύ,α);}else{Vector3D ƥ=ï.ʙ.Ũ+(ï.ʙ
.ţ*(s-ï.ʙ.ŧ+1)*ś);MyIni ΰ=new MyIni();ΰ.Set(ϋ,"EntityId",ï.ʙ.ũ);ΰ.Set(ϋ,"PositionX",ƥ.X);ΰ.Set(ϋ,"PositionY",ƥ.Y);ΰ.Set(ϋ
,"PositionZ",ƥ.Z);ΰ.Set(ϋ,"VelocityX",ï.ʙ.ţ.X);ΰ.Set(ϋ,"VelocityY",ï.ʙ.ţ.Y);ΰ.Set(ϋ,"VelocityZ",ï.ʙ.ţ.Z);ï.ӓ.TryRun(ϋ+ΰ.
ToString());}ï.Ӗ=s+Ň.Ӄ;Ѣ=true;}if(!ï.Ӕ){ѥ.Enqueue(ï);}break;}}}void ί(){if(s>=Ѡ&&Ѥ.Count==0){if(ы.ʋ()>0){List<Ū>ˣ=ы.ʈ();ˣ.Sort(Ϣ
);foreach(Ū Á in ˣ){Ѥ.Enqueue(Á);}}Ѡ=s+Ň.ӥ;}while(Ѥ.Count>0){Ū Á=Ѥ.Dequeue();if(ы.ʎ(Á.ũ,s-Ň.Ӿ)){MyTuple<long,Vector3D,
Vector3D,double,int,long>ά=new MyTuple<long,Vector3D,Vector3D,double,int,long>();ά.Item1=Á.ũ;ά.Item2=Á.Ũ+(Á.ţ*(s-Á.ŧ+1)*ś);ά.
Item3=Á.ţ;ά.Item4=Á.Ɖ;if(Á.Ŵ)ά.Item5|=(int)ѿ.Ŵ;ά.Item6=0;IGC.SendBroadcastMessage(Ϗ,ά);Ѣ=true;break;}}}void έ(){if(s>=Ѩ){
MyTuple<long,Vector3D,Vector3D,double,int,long>ά=new MyTuple<long,Vector3D,Vector3D,double,int,long>();ά.Item1=Me.CubeGrid.
EntityId;ά.Item2=Me.CubeGrid.WorldAABB.Center;ά.Item3=(ϛ!=null?ϛ.GetShipVelocities().LinearVelocity:Vector3D.Zero);ά.Item4=ˇ*ˇ*4
;ά.Item5|=(int)ѿ.Ұ;if(Me.CubeGrid.GridSizeEnum==MyCubeSize.Large)ά.Item5|=(int)ѿ.Ŵ;ά.Item6=0;IGC.SendBroadcastMessage(Ϗ,ά
);Ѩ=s+Ň.ӥ;}}void Ϋ(){foreach(ӏ Α in т){if(Α.Ӌ!=null){IMyTextSurface Ϊ=Α.Ӌ;Vector2 Ω;Vector2 Ψ;if(Ϊ.SurfaceSize.X==Ϊ.
SurfaceSize.Y){Ω=(Ϊ.TextureSize-Ϊ.SurfaceSize)*0.5f;Ψ=new Vector2(Ϊ.SurfaceSize.X)*0.0009765625f;}else if(Ϊ.SurfaceSize.X>Ϊ.
SurfaceSize.Y){Ω=(Ϊ.TextureSize-Ϊ.SurfaceSize+new Vector2(Ϊ.SurfaceSize.X-Ϊ.SurfaceSize.Y,0f))*0.5f;Ψ=new Vector2(Ϊ.SurfaceSize.Y)*
0.0009765625f;}else{Ω=(Ϊ.TextureSize-Ϊ.SurfaceSize+new Vector2(0f,Ϊ.SurfaceSize.Y-Ϊ.SurfaceSize.X))*0.5f;Ψ=new Vector2(Ϊ.SurfaceSize.
Y)*0.0009765625f;}List<MySprite>Χ=new List<MySprite>();Χ.Add(new MySprite(SpriteType.TEXTURE,"SquareSimple",Ϊ.TextureSize
*0.5f,null,Color.Black));Vector2 Φ=Ψ*new Vector2(1024f,100f);Vector2 п=new Vector2(Φ.X*0.5f,0f);Vector2 Υ=Φ*0.5f;Vector2
Ѫ=new Vector2(0f,Φ.Y);Vector2 Ն=new Vector2(0f,Ψ.Y*20f);Vector2 Մ=Ψ*new Vector2(480f,100f);Vector2 Ճ=new Vector2(Մ.X*0.5f
,0f);Vector2 Ղ=Մ*0.5f;Vector2 Ձ=Ψ*new Vector2(544f,0f);float Հ=ω*Φ.Y;Vector2 Կ=Ω+Ն;string ԑ;Color Ծ;Color Խ;Χ.Add(new
MySprite(SpriteType.TEXTURE,"SquareSimple",Կ+Υ,Φ,χ));Χ.Add(new MySprite(SpriteType.TEXT,$"{Ͽ} v{Ѐ}",Կ+п,null,ψ,"DEBUG",
TextAlignment.CENTER,Հ));Կ+=Ѫ;Χ.Add(new MySprite(SpriteType.TEXTURE,"SquareSimple",Կ+Υ,Φ,υ));Χ.Add(new MySprite(SpriteType.TEXT,
$"Manual Targeter {Α.ӎ}",Կ+п,null,φ,"DEBUG",TextAlignment.CENTER,Հ));Կ+=Ѫ+Ն;Χ.Add(new MySprite(SpriteType.TEXT,"Status",Կ+п,null,ϕ,"DEBUG",
TextAlignment.CENTER,Հ));Կ+=Ѫ;if(Α.ӈ>0){ԑ="Target Locked";Ծ=ϩ;Խ=Ϩ;}else if(Α.Ӊ){ԑ="Seeking";Ծ=Ϭ;Խ=Ϫ;}else{ԑ="No Target";Ծ=τ;Խ=ϗ;}Χ.
Add(new MySprite(SpriteType.TEXTURE,"SquareSimple",Կ+Υ,Φ,Խ));Χ.Add(new MySprite(SpriteType.TEXT,ԑ,Կ+п,null,Ծ,"DEBUG",
TextAlignment.CENTER,Հ));Կ+=Ѫ+Ն;Χ.Add(new MySprite(SpriteType.TEXT,"Current Target",Կ+п,null,ϕ,"DEBUG",TextAlignment.CENTER,Հ));Կ+=Ѫ;
if(Α.ӈ>0){ԑ=Ա(Α.ӈ);Ծ=ϧ;Խ=Ϧ;}else{ԑ="-";Ծ=τ;Խ=ϗ;}Χ.Add(new MySprite(SpriteType.TEXTURE,"SquareSimple",Կ+Υ,Φ,Խ));Χ.Add(new
MySprite(SpriteType.TEXT,ԑ,Կ+п,null,Ծ,"DEBUG",TextAlignment.CENTER,Հ));Կ+=Ѫ+Ն;Χ.Add(new MySprite(SpriteType.TEXT,"Range",Կ+Ճ,
null,ϕ,"DEBUG",TextAlignment.CENTER,Հ));Χ.Add(new MySprite(SpriteType.TEXT,"Hit Point",Կ+Ձ+Ճ,null,ϕ,"DEBUG",TextAlignment.
CENTER,Հ));Կ+=Ѫ;ԑ=$"{Math.Round(Α.ò*0.001,1):n1}km";Ծ=ϥ;Խ=Ϥ;Χ.Add(new MySprite(SpriteType.TEXTURE,"SquareSimple",Կ+Ղ,Մ,Խ));Χ.
Add(new MySprite(SpriteType.TEXT,ԑ,Կ+Ճ,null,Ծ,"DEBUG",TextAlignment.CENTER,Հ));ԑ=Α.ū();Ծ=ϥ;Խ=Ϥ;Χ.Add(new MySprite(
SpriteType.TEXTURE,"SquareSimple",Կ+Ձ+Ղ,Մ,Խ));Χ.Add(new MySprite(SpriteType.TEXT,ԑ,Կ+Ձ+Ճ,null,Ծ,"DEBUG",TextAlignment.CENTER,Հ));Ϊ
.ContentType=ContentType.SCRIPT;Ϊ.Script="";MySpriteDrawFrame Լ=Ϊ.DrawFrame();Լ.AddRange(Χ);Լ.Dispose();}}}void Ի(){
foreach(var Ժ in Զ){IMyTextSurface Թ=Ժ as IMyTextSurface;RectangleF Ը=new RectangleF((Թ.TextureSize-Թ.SurfaceSize)/2f,Թ.
SurfaceSize);Ս.Ԕ(Ժ);Յ(Ժ.DrawFrame(),Ժ.CustomData,Ը);}}void ӱ(){if(Ň.ӱ==true){if(ѝ==true){Me.CustomName=Ň.ө;}else if(ѝ==false){Me.
CustomName=Ň.Ө;}}}void Ӌ(){á.Clear();if(ѝ){á.AppendLine($"====[ Diamond Dome System ]===");}else{á.AppendLine(
$"====[       DISABLED      ]===");}bool Է=Ň.Ӫ!=null;á.AppendLine($"WeaponCoreAPI : {Է}\n");á.AppendLine($"Tracked Targets : {ы.ʋ()}");á.AppendLine(
$"PDCs : {Ϛ.Count(ȝ=>{return!ȝ.Ļ;})}");á.AppendLine($"Designators : {ђ.Count}");á.AppendLine($"Raycast Cameras : {ю.Count}");á.AppendLine(
$"Guided Missiles : {ф.Count}");á.AppendLine($"Guided Torpedos : {у.Count}");á.AppendLine("\n---- Runtime Performance ---\n");ќ.â(á);if(ћ){á.
AppendLine("\n>>>>>>> Debug Mode <<<<<<<");á.AppendLine(ϟ.ToString());á.AppendLine("\n---- Debug Performance ---\n");ќ.ñ(á);}Echo(
á.ToString());}List<IMyTextPanel>Զ=new List<IMyTextPanel>();void Յ(MySpriteDrawFrame Ե,string Շ,RectangleF ՙ,bool Օ=false
){bool Է=Ň.Ӫ!=null;string Ք=$"PDCs : {Ϛ.Count(ȝ=>{return!ȝ.Ļ;})}\n";string Փ=$"Designators : {ђ.Count}\n";string Ւ=
$"Raycast Cameras : {ю.Count}\n";string Ց=$"Guided Missiles : {ф.Count}\n";string Ր=$"Guided Torpedos : {у.Count}\n";string Տ=
$"Tracked Targets : {ы.ʋ()}\n";var Ֆ=Շ;var Վ=Ֆ.Split('\n');var Ռ=Վ.Length;for(int Ǝ=0;Ǝ<Ռ;Ǝ++){string ԑ="";if(Վ[Ǝ].ToLower().Contains("pdc")){ԑ=Ք;}
else if(Վ[Ǝ].ToLower().Contains("designator")){ԑ=Փ;}else if(Վ[Ǝ].ToLower().Contains("cameras")){ԑ=Ւ;}else if(Վ[Ǝ].ToLower().
Contains("missiles")){ԑ=Ց;}else if(Վ[Ǝ].ToLower().Contains("torpedos")){ԑ=Ր;}else if(Վ[Ǝ].ToLower().Contains("targets")){ԑ=Տ;}
else{continue;}var Ջ=(ՙ.Height/Ռ)*0.05f;Vector2 Ԝ=new Vector2(ՙ.Width*0.9f,(ՙ.Height/Ռ)*0.9f);float Պ=((ՙ.Height/(Ռ+1))*(Ǝ+1
))-(Ԝ.Y/2)-Ջ;MySprite Չ=Ս.Ԍ(Ս.Դ.ԕ,new Vector2(ՙ.Width*0.05f,Պ),Ԝ,0f,Color.Black);Ե.Add(Չ);float Ž=ՙ.Width*1*0.9f;float ż=
ՙ.Height/Ռ*0.9f;Vector2 Ո=new Vector2(Ž,ż);Ե.Add(Ս.Ԍ(Ս.Դ.Ԗ,(Vector2)Չ.Position,Ո,0f,Color.Black));Ե.Add(Ս.Ԓ(ԑ,new Vector2
(ՙ.Center.X,Չ.Position.Value.Y-(Ԝ.Y/3)),Օ?0.8f:2f,TextAlignment.CENTER,Color.NavajoWhite));}Ե.Dispose();}static class Ս{
public enum Դ{ԣ,ԛ,Ԛ,ԙ,Ԙ,ԗ,Ԗ,ԕ}public static void Ԕ(IMyTextSurface ԓ){ԓ.ContentType=ContentType.SCRIPT;ԓ.ScriptBackgroundColor=
new Color(0,0,0);ԓ.Script="";}public static MySprite Ԓ(string ԑ,Vector2 ɢ,float Ԑ,TextAlignment ԏ,Color Ԏ){var ԍ=new
MySprite(){Type=SpriteType.TEXT,Data=ԑ,Position=ɢ,RotationOrScale=Ԑ,Color=Ԏ,Alignment=ԏ,FontId="Blue"};return ԍ;}public static
MySprite Ԍ(Դ ԋ,Vector2 ɢ,Vector2 Ԝ,float Ԋ,Color Ԏ){var ԍ=new MySprite(){Type=SpriteType.TEXTURE,Data=ԋ.ToString(),Position=ɢ,
Size=Ԝ,RotationOrScale=Ԋ,Color=Ԏ};return ԍ;}}long Բ(){ϫ.NextBytes(р);Buffer.BlockCopy(р,0,щ,0,8);return щ[0];}string Ա(long
ƈ){return$"T{ƈ%100000:00000}";}string ԧ(Vector3D Ԧ){return Convert.ToBase64String(BitConverter.GetBytes((float)Ԧ.X))+
Convert.ToBase64String(BitConverter.GetBytes((float)Ԧ.Y))+Convert.ToBase64String(BitConverter.GetBytes((float)Ԧ.Z));}bool ԥ(
IMyTerminalBlock Ύ){return(Ύ!=null&&Ύ.IsWorking);}bool Ԥ(ref MyDetectedEntityInfo ɶ){if(ɶ.Type==MyDetectedEntityType.LargeGrid||ɶ.Type==
MyDetectedEntityType.SmallGrid){return Գ(ref ɶ);}return false;}bool Գ(ref MyDetectedEntityInfo ɶ){return!(ɶ.Relationship==
MyRelationsBetweenPlayerAndBlock.Owner||ɶ.Relationship==MyRelationsBetweenPlayerAndBlock.FactionShare);}Ȭ Ԣ(IMyUserControllableGun q){if(q.
BlockDefinition.SubtypeId.Contains("Gatling")){return Ϝ.Ґ;}else if(q.BlockDefinition.SubtypeId.Contains("Missile")||q.BlockDefinition.
SubtypeId.Contains("Rocket")){return Ϝ.ҏ;}return Ϝ.Ґ;}bool ԡ(Ҙ ˀ){return ˀ.Ҏ.HasTarget;}bool Ԡ(Ҙ ˀ){return ˀ.Ҏ.IsWorking;}bool ԟ(
Ż ˎ){return!ˎ.Ļ;}bool Ԟ(IMyTerminalBlock Ύ,string ԝ){return(Ύ.CustomName.IndexOf(ԝ,StringComparison.OrdinalIgnoreCase)>-1
);}bool Ԟ(IMyBlockGroup ɼ,string ԝ){return(ɼ.Name.IndexOf(ԝ,StringComparison.OrdinalIgnoreCase)>-1);}bool վ(string[]π,
string ԝ){foreach(string ս in π){if(ս.Trim().Equals(ԝ,StringComparison.OrdinalIgnoreCase)){return true;}}return false;}void ռ<
ɝ>(out List<ɝ>ʪ,string ջ,Func<ɝ,bool>տ=null)where ɝ:class{ʪ=null;List<IMyBlockGroup>ˤ=new List<IMyBlockGroup>();
GridTerminalSystem.GetBlockGroups(ˤ,(ȝ)=>{return ȝ.Name.IndexOf(ջ,StringComparison.OrdinalIgnoreCase)>-1;});foreach(IMyBlockGroup ɼ in ˤ){
List<ɝ>Ф=new List<ɝ>();ɼ.GetBlocksOfType(Ф,տ);if(ʪ==null){ʪ=Ф;}else{ʪ.AddList(Ф);}}}
}public interface չ{bool ո(Vector3 Ѵ,Vector3 Î);}class շ:չ{public Vector3I[]ն={Vector3I.Left,Vector3I.Right,Vector3I.Up,
Vector3I.Down,Vector3I.Forward,Vector3I.Backward};public MyDynamicAABBTree պ;public IMyCubeGrid ɕ;public շ(){}public IEnumerator
<int>ב(Ӓ ҧ,List<Ӓ>Ҧ,float ҥ,int և){if(և<=0)և=1000000000;int ֆ=0;ɕ=ҧ.ɕ;պ=new MyDynamicAABBTree();Vector3 օ=new Vector3(
0.5f*ɕ.GridSize);if(ҥ!=0f){օ+=new Vector3(ҥ);}Stack<Vector3I>ք=new Stack<Vector3I>();HashSet<Vector3I>փ=new HashSet<Vector3I
>();BoundingBox ւ;ք.Push(ҧ.ә);փ.Add(ҧ.ә);ւ=new BoundingBox((ҧ.ә*ɕ.GridSize)-օ,(ҧ.ә*ɕ.GridSize)+օ);պ.AddProxy(ref ւ,ւ,0);
Vector3I א;while(ք.Count>0){Vector3I ï=ք.Pop();for(int Ǝ=0;Ǝ<6;Ǝ++){א=ï+ն[Ǝ];if(!փ.Contains(א)){փ.Add(א);if(ɕ.CubeExists(א)){ք.
Push(א);ւ=new BoundingBox((א*ɕ.GridSize)-օ,(א*ɕ.GridSize)+օ);պ.AddProxy(ref ւ,ւ,0);}}}ֆ++;if(ֆ%և==0){yield return ֆ;}}
Dictionary<IMyCubeGrid,Ӓ>ң=new Dictionary<IMyCubeGrid,Ӓ>();foreach(Ӓ ѹ in Ҧ){if(!ң.ContainsKey(ѹ.ɕ)){ң.Add(ѹ.ɕ,ѹ);}}MatrixD ѻ=ɕ.
WorldMatrix;MatrixD.Transpose(ref ѻ,out ѻ);foreach(Ӓ ѹ in ң.Values){if(ѹ.ɕ!=ɕ){Vector3 ց=Vector3D.TransformNormal((ѹ.ɕ.WorldAABB.
Min-ɕ.WorldMatrix.Translation),ref ѻ);Vector3 ր=Vector3D.TransformNormal((ѹ.ɕ.WorldAABB.Max-ɕ.WorldMatrix.Translation),ref
ѻ);ւ=new BoundingBox(ց-օ,ր+օ);պ.AddProxy(ref ւ,ւ,0);}}}public bool ո(Vector3 Ѵ,Vector3 Î){MatrixD ѻ=ɕ.WorldMatrix;MatrixD
.Transpose(ref ѻ,out ѻ);Line ѷ=new Line(Vector3D.TransformNormal(Ѵ-ɕ.WorldMatrix.Translation,ref ѻ),Vector3D.
TransformNormal(Î,ref ѻ)*1000);return Ѹ(ref ѷ);}public bool Ѹ(ref Line ѷ){List<MyLineSegmentOverlapResult<BoundingBox>>ʪ=new List<
MyLineSegmentOverlapResult<BoundingBox>>(0);պ.OverlapAllLineSegment(ref ѷ,ʪ);foreach(MyLineSegmentOverlapResult<BoundingBox>խ in ʪ){if(խ.Element.
Extents.LengthSquared()<(ѷ.From-խ.Element.Center).LengthSquared()){return true;}}return false;}}static class լ{const string ի=
"AGMSAVE";const string ժ="[NOTREADY]";public static bool թ(IMyProgrammableBlock ը){if(ը!=null&&ը.CustomName.IndexOf(ժ,
StringComparison.OrdinalIgnoreCase)>-1){Vector3I[]ե={-Base6Directions.GetIntVector(ը.Orientation.Left),Base6Directions.GetIntVector(ը.
Orientation.Up),-Base6Directions.GetIntVector(ը.Orientation.Forward)};MyIni Л=new MyIni();if(Л.TryParse(ը.CustomData)&&Л.
ContainsSection(ի)){char[]զ={','};if(!է("DetachBlock",ը,Л,զ,ref ե,true))return false;if(!է("DampenerBlock",ը,Л,զ,ref ե,true))return
false;if(!է("ForwardBlock",ը,Л,զ,ref ե,false))return false;if(!է("RemoteControl",ը,Л,զ,ref ե,false))return false;if(!է(
"Gyroscopes",ը,Л,զ,ref ե,false))return false;if(!է("Thrusters",ը,Л,զ,ref ե,false))return false;if(!է("PowerBlocks",ը,Л,զ,ref ե,true)
)return false;if(!է("RaycastCameras",ը,Л,զ,ref ե,true))return false;}ը.CustomName=ը.CustomName.Replace(ժ,"").Trim();}
return true;}static bool է(string ó,IMyTerminalBlock ա,MyIni Л,char[]զ,ref Vector3I[]ե,bool դ){string[]գ=Л.Get(ի,ó).ToString()
.Split(զ,StringSplitOptions.RemoveEmptyEntries);return((գ.Length>0||դ)&&բ(գ,ա,ref ե));}static bool բ(string[]ծ,
IMyTerminalBlock ա,ref Vector3I[]ե){foreach(string ѷ in ծ){if(ѷ!=null&&ѷ.Length==12){Vector3I ʪ=new Vector3I();ʪ.X=BitConverter.ToInt16(
Convert.FromBase64String(ѷ.Substring(0,4)),0);ʪ.Y=BitConverter.ToInt16(Convert.FromBase64String(ѷ.Substring(4,4)),0);ʪ.Z=
BitConverter.ToInt16(Convert.FromBase64String(ѷ.Substring(8,4)),0);ʪ=(ʪ.X*ե[0])+(ʪ.Y*ե[1])+(ʪ.Z*ե[2]);ʪ+=ա.Position;if(!ա.CubeGrid.
CubeExists(ʪ)){return false;}}else{return false;}}return true;}}class յ{Dictionary<long,ʜ>մ;SortedSet<ʜ>ճ;public յ(){մ=new
Dictionary<long,ʜ>();ճ=new SortedSet<ʜ>(new қ());}public bool ղ(ref MyDetectedEntityInfo ɶ,int s){if(մ.ContainsKey(ɶ.EntityId)){ʜ
ʌ=մ[ɶ.EntityId];ʌ.Ҝ.Ũ=ɶ.Position;ʌ.Ҝ.ţ=ɶ.Velocity;ʌ.Ҝ.Ŵ=(ɶ.Type==MyDetectedEntityType.LargeGrid);ʌ.Ҝ.Ң=ɶ.BoundingBox.
Extents.LengthSquared();ʌ.Ҝ.Š=s;ճ.Remove(ʌ);ʌ.ʥ=s;ճ.Add(ʌ);return false;}else{Қ ː=new Қ(ɶ.EntityId);ː.Ũ=ɶ.Position;ː.ţ=ɶ.
Velocity;ː.Š=s;ʜ ʌ=new ʜ();ʌ.ũ=ː.ũ;ʌ.Ҝ=ː;ʌ.ʥ=s;մ.Add(ɶ.EntityId,ʌ);ճ.Add(ʌ);return true;}}public bool ղ(Қ ձ,int s){if(մ.
ContainsKey(ձ.ũ)){ʜ ʌ=մ[ձ.ũ];ʌ.Ҝ.Ũ=ձ.Ũ;ʌ.Ҝ.ţ=ձ.ţ;ʌ.Ҝ.Š=s;ճ.Remove(ʌ);ʌ.ʥ=s;ճ.Add(ʌ);return false;}else{ձ.Š=s;ʜ ʌ=new ʜ();ʌ.ũ=ձ.ũ;ʌ.
Ҝ=ձ;ʌ.ʥ=s;մ.Add(ձ.ũ,ʌ);ճ.Add(ʌ);return true;}}public void հ(long ƈ,int s){if(մ.ContainsKey(ƈ)){ʜ ʌ=մ[ƈ];ճ.Remove(ʌ);ʌ.ʥ=s
;ճ.Add(ʌ);}}public bool կ(long ƈ){return մ.ContainsKey(ƈ);}public int ʋ(){return մ.Count;}public Қ Ӝ(long ƈ){ʜ ʌ;if(մ.
TryGetValue(ƈ,out ʌ))return ʌ.Ҝ;else return null;}public List<Қ>Ҡ(){List<Қ>ʇ=new List<Қ>(մ.Count);foreach(ʜ ʌ in ճ){ʇ.Add(ʌ.Ҝ);}
return ʇ;}public Қ ҟ(){if(ճ.Count==0)return null;else return ճ.Min.Ҝ;}public void Ҟ(long ƈ){if(մ.ContainsKey(ƈ)){ճ.Remove(մ[ƈ]
);մ.Remove(ƈ);}}public void ҝ(){մ.Clear();ճ.Clear();}class ʜ{public long ũ;public int ʥ;public Қ Ҝ;}class қ:IComparer<ʜ>{
public int Compare(ʜ Ž,ʜ ż){if(Ž==null)return(ż==null?0:1);else if(ż==null)return-1;else return(Ž.ʥ<ż.ʥ?-1:(Ž.ʥ>ż.ʥ?1:(Ž.ũ<ż.ũ
?-1:(Ž.ũ>ż.ũ?1:0))));}}}public class Қ{public long ũ;public Vector3D Ũ;public Vector3D ţ;public bool Ŵ;public double Ң;
public int Š;public Қ(long ƈ){ũ=ƈ;}}class Ү:չ{private int ҭ=40;public IMyCubeGrid ɕ;public BoundingBox Ҭ;double ҫ;public List<
IMyCubeGrid>Ҫ;public float ҩ;public bool ү;public Ү(Ӓ ҧ,List<Ӓ>Ҧ,float ҥ,IMyProgrammableBlock Ҥ){ɕ=ҧ.ɕ;Ҭ=new BoundingBox(ɕ.Min,ɕ.
Max);ҫ=1.0/ɕ.GridSize;if(ҥ!=0){ү=true;ҩ=ɕ.GridSize*0.5f+ҥ;}else{ү=false;ҩ=0f;}Dictionary<IMyCubeGrid,Ӓ>ң=new Dictionary<
IMyCubeGrid,Ӓ>();foreach(Ӓ ѹ in Ҧ){if(!ң.ContainsKey(ѹ.ɕ)){ң.Add(ѹ.ɕ,ѹ);}}Ҫ=new List<IMyCubeGrid>(ң.Count);MatrixD ѻ=ɕ.WorldMatrix;
MatrixD.Transpose(ref ѻ,out ѻ);foreach(Ӓ ѹ in ң.Values){if(ѹ.ɕ!=ɕ){Ҫ.Add(ѹ.ɕ);}}}public bool ո(Vector3 Ѵ,Vector3 Î){float Ҩ;if(
ɕ.WorldAABB.Contains(Ѵ)==ContainmentType.Disjoint){double?ʪ=ɕ.WorldAABB.Intersects(new Ray(Ѵ,Î));if(ʪ==null){return false
;}Ҩ=(float)ʪ.Value;}else{double?ʪ=ɕ.WorldAABB.Intersects(new Ray(Ѵ+(Î*5000),-Î));if(ʪ==null){return false;}Ҩ=5000f-(float
)ʪ.Value;}MatrixD ѻ=ɕ.WorldMatrix;MatrixD.Transpose(ref ѻ,out ѻ);Line ѷ=new Line(Vector3D.TransformNormal(Ѵ-ɕ.WorldMatrix
.Translation,ref ѻ)*ҫ,Vector3D.TransformNormal(Ѵ+(Î*Ҩ)-ɕ.WorldMatrix.Translation,ref ѻ)*ҫ);if(Ѹ(ref ѷ)){return true;}if(Ҫ
.Count>0){RayD Ѻ=new RayD(Ѵ,Î);foreach(IMyCubeGrid ѹ in Ҫ){if(ѹ.WorldAABB.Intersects(Ѻ)!=null){if(ѹ.WorldAABB.Extents.
LengthSquared()<(Ѻ.Position-ѹ.WorldAABB.Center).LengthSquared()){return true;}}}}return false;}public bool Ѹ(ref Line ѷ){int Ѷ=Math.
Min((int)Math.Ceiling(ѷ.Length),ҭ);float ѵ=1.0f/Ѷ*ѷ.Length;Vector3I Ѵ=new Vector3I((int)Math.Round(ѷ.From.X),(int)Math.
Round(ѷ.From.Y),(int)Math.Round(ѷ.From.Z));for(int Ǝ=1;Ǝ<=Ѷ;Ǝ++){Vector3 ѳ=ѷ.From+(ѷ.Direction*ѵ*Ǝ);Vector3I Ѳ=new Vector3I((
int)Math.Round(ѳ.X),(int)Math.Round(ѳ.Y),(int)Math.Round(ѳ.Z));if(ɕ.CubeExists(Ѳ)){if(Ѳ!=Ѵ){if(ү){double Ρ=Math.Min(Math.
Abs(ѳ.X-Ѳ.X),Math.Min(Math.Abs(ѳ.Y-Ѳ.Y),Math.Abs(ѳ.Z-Ѳ.Z)));if(Ρ<=ҩ){return true;}}else{return true;}}}}return false;}}
class ѱ{const double Ѱ=400;const double ѯ=0;const double Ѯ=400;const double ѭ=800;const double Ѭ=0;const double Ѽ=0;const
bool ѫ=false;const bool Ѿ=false;const double ҙ=100;const double җ=600;const double Җ=200;const double ҕ=800;const double Ҕ=4
;const double ғ=1;const bool Ғ=true;const bool ґ=true;public Ȭ Ґ=new Ȭ(Ѱ,ѯ,Ѯ,ѭ,Ѭ,Ѽ,ѫ,Ѿ);public Ȭ ҏ=new Ȭ(ҙ,җ,Җ,ҕ,Ҕ,ғ,Ғ,ґ)
;}class Ҙ{public IMyLargeTurretBase Ҏ;public float ҍ;public ITerminalProperty<float>Ҍ;public int ҋ=0;public int Ҋ=0;
Random ϫ=new Random();public Ҙ(IMyLargeTurretBase м){Ҏ=м;ҍ=м.GetMaximum<float>("Range");Ҍ=м.GetProperty("Range").As<float>();}
public void ҁ(){Ҍ.SetValue(Ҏ,ҍ-0.01f);Ҍ.SetValue(Ҏ,ҍ);}}public enum Ҁ{ѽ=1}public enum ѿ{Ұ=1,Ŵ=2}public enum Ӱ{ӯ=0,Ӯ=1,ӭ=2}
public class Ӭ{public Program ӫ;public ʗ Ӫ;public string ө="PB | DDS enabled";public string Ө="PB | DDS disabled";public bool
ӱ=false;public int ӧ=600;public int ӥ=45;public int Ӥ=3600;public double ӣ=5000;public int Ӣ=30;public int ӡ=1;public
float Ӡ=1;public int ӟ=15;public bool Ӟ=true;public int ӝ=45;public bool Ӧ=true;public int Ӳ=2;public int ӽ=3;public int ԉ=60
;public bool ԇ=true;public double Ԇ=12;public int ԅ=15;public double Ԅ=3000;public int ԃ=15;public int Ԃ=3;public int ԁ=
15;public int Ԁ=90;public int ӿ=2;public int Ԉ=15;public int Ӿ=60;public double Ӽ=5;public double ӻ=2;public double Ӻ=4;
public bool ӹ=true;public double Ӹ=12;public double ӷ=36;public double Ӷ=2000;public double ӵ=0.5;public double Ӵ=0.75;public
double ӳ=800;public int ӛ=60;public int ӆ=600;public int ӑ=300;public int ӄ=600;public int Ӄ=15;public double ӂ=60;public
double Ӂ=0.995;public bool Ӏ=true;public bool ҿ=false;public float Ҿ=1f;public float ҽ=60f;public float Ҽ=60f;public float һ=
50f;public float Һ=60f;public int ҹ=90;public int Ҹ=300;public double ҷ=88;public double Ҷ=79;public int ҵ=30;public int Ҵ=
120;public bool ҳ=true;public bool Ҳ=false;public float Ӆ=0f;public bool ұ=false;public int Ӈ=500;}class Ӛ{public long Ә;
public Ū ʙ;public int ӗ;public int Ӗ;public bool ӕ;public bool Ӕ;public IMyProgrammableBlock ӓ;}public class Ӓ{public
IMyCubeGrid ɕ;public Vector3I ә;public Ӓ(IMyCubeGrid Ӑ,Vector3I Ѵ){ɕ=Ӑ;ә=Ѵ;}}class ӏ:Ū{public string ӎ;public IMyTerminalBlock Ӎ;
public IMyLargeTurretBase ӌ;public IMyTextSurface Ӌ;public IMyTerminalBlock ӊ;public bool Ӊ=false;public long ӈ=-1;public
Vector3D?ʧ;public Ӱ ų=Ӱ.ӯ;public double ò;public ӏ(string ű,IMyTerminalBlock Ű,IMyTextSurface ů,IMyTerminalBlock Ů):base(-1){ӎ=ű
;Ӎ=Ű;ӌ=Ű as IMyLargeTurretBase;Ӌ=ů;ӊ=Ů;}public Vector3D ŭ(){if(Ӎ==null){return Vector3D.Zero;}if(ӌ!=null){Vector3D Ŭ;
Vector3D.CreateFromAzimuthAndElevation(ӌ.Azimuth,ӌ.Elevation,out Ŭ);return Vector3D.TransformNormal(Ŭ,ӌ.WorldMatrix);}else{
return Ӎ.WorldMatrix.Forward;}}public string ū(){switch(ų){case Ӱ.ӯ:return"Center";case Ӱ.Ӯ:return"Offset";case Ӱ.ӭ:return
"Random";}return"-";}}public class Ū{public long ũ;public Vector3D Ũ;public int ŧ;public Vector3D Ŧ;public int ť;public int Ť;
public Vector3D ţ;public Vector3D Ţ;public Vector3D š;public int Š;public MatrixD?Ų;public int ş;public bool Ŵ;public double Ɖ
;public double Ƈ;public double Ɔ;public double ƅ;public double Ƅ;public double ƃ;public int Ƃ;public int Ɓ;public List<ʧ>
ƀ;public Ū(long ƈ){ũ=ƈ;}}class ſ:IComparer<Ū>{public int Compare(Ū Ž,Ū ż){if(Ž==null)return(ż==null?0:1);else if(ż==null)
return-1;else return(Ž.ŧ<ż.ŧ?-1:(Ž.ŧ>ż.ŧ?1:(Ž.ũ<ż.ũ?-1:(Ž.ũ>ż.ũ?1:0))));}}public class Ż{public Ӭ Ň;public MyIni ź;public
string Ź;public IMyMotorStator Ÿ;public double ŷ;public double Ŷ;public double ŵ;public bool ž;public double Ş;public Č ő;
public f[]Ņ;public Ȭ ń;public double Ń;public float ł;public IMyShipController Ł;public ITerminalProperty<bool>ŀ;public
ITerminalAction Ŀ;public չ ľ;public double Ľ;public Ū ļ;public bool Ļ;public IMyShipConnector ĺ;public IMyShipConnector Ĺ;public int ĸ;
public int ķ;public int Ķ;public Ē ĵ;public double Ĵ;public double ĳ;long ņ;Vector3D Ĳ;const int ň=Program.ň;const int ŝ=
Program.ŝ;const double ś=Program.ś;const string Ś=Program.Ś;const int ř=Program.ř;const double Ř=Program.Ř;double ŗ=Math.Tan(
MathHelperD.ToRadians(ň));long Ŗ;int ŕ;Vector3D Ŕ;Vector3D œ;double Ŝ;double Œ;double Ő;public Ż(string ŏ,IMyMotorStator Ŏ,
IMyMotorStator U,IMyTerminalBlock ō,IMyShipController Ō,List<IMyUserControllableGun>ŋ,Ȭ Ŋ,Ӭ Ň,չ ŉ=null):this(ŏ,Ŏ,new List<
IMyMotorStator>(new IMyMotorStator[]{U}),new List<IMyTerminalBlock>(new IMyTerminalBlock[]{ō}),Ō,new List<List<IMyUserControllableGun>
>(){ŋ},Ŋ,Ň,ŉ){}public Ż(string ŏ,IMyMotorStator Ŏ,List<IMyMotorStator>Ñ,List<IMyTerminalBlock>ƶ,IMyShipController Ō,List<
List<IMyUserControllableGun>>ŋ,Ȭ Ŋ,Ӭ Ň,չ ŉ=null){this.Ň=Ň;Ź=ŏ;Ł=Ō;ń=Ŋ;ł=MathHelper.RPMToRadiansPerSecond*(this.Ň.ҿ?this.Ň.Һ:
this.Ň.Ҽ);ľ=ŉ;ź=new MyIni();if(ź.TryParse(Ŏ.CustomData)){if(ź.ContainsSection(Ś)){double Ƶ=ź.Get(Ś,"WeaponInitialSpeed").
ToDouble(0);double Ʒ=ź.Get(Ś,"WeaponAcceleration").ToDouble(0);double ƴ=ź.Get(Ś,"WeaponMaxSpeed").ToDouble(0);double Ʋ=ź.Get(Ś,
"WeaponMaxRange").ToDouble(0);double Ʊ=ź.Get(Ś,"WeaponSpawnOffset").ToDouble(0);double ư=ź.Get(Ś,"WeaponReloadTime").ToDouble(0);bool Ư=
ź.Get(Ś,"WeaponIsCappedSpeed").ToBoolean(false);bool ƭ=ź.Get(Ś,"WeaponUseSalvo").ToBoolean(false);Ń=ź.Get(Ś,
"TurretPrioritySize").ToDouble(0);if(Ʋ>0&&(Ƶ>0||Ʒ>0)){Ƶ=MathHelper.Clamp(Ƶ,0,1000000000);Ʒ=MathHelper.Clamp(Ʒ,0,1000000000);ƴ=MathHelper.
Clamp(ƴ,0,1000000000);if(ƴ==0)ƴ=1000000000;Ʋ=MathHelper.Clamp(Ʋ,0,1000000000);ń=new Ȭ(Ƶ,Ʒ,ƴ,Ʋ,Ʊ,ư,Ư,ƭ);}}}else{Ń=0;}if(ŋ.
Count>0&&ŋ[0].Count>0){ŀ=ŋ[0][0].GetProperty("Shoot").As<bool>();Ŀ=ŋ[0][0].GetActionWithName("ShootOnce");}Ÿ=Ŏ;ž=M(Ÿ,out Ŷ,
out ŵ);if(!this.Ň.ҿ){ő=new Č(this.Ň.ҽ,this.Ň.Ҿ,ł);D(Ÿ,Ŷ,ŵ);}Ņ=new f[Ñ.Count];for(int Ǝ=0;Ǝ<Ñ.Count;Ǝ++){f U=new f();Ņ[Ǝ]=U;
U.e=Ñ[Ǝ];U.Û=ƶ[Ǝ];double Ƭ,ƫ;M(U.e,out Ƭ,out ƫ);U.W=(Ƭ>double.MinValue&&ƫ<double.MaxValue);U.Y=Ƭ;U.X=ƫ;ƹ(U);if(!this.Ň.ҿ)
{U.ę=new Č(this.Ň.ҽ,this.Ň.Ҿ,ł);D(U.e,Ƭ,ƫ);}U.Ę=ŋ[Ǝ];if(ń.ȓ){U.ē=(int)Math.Ceiling((ń.ȕ*ŝ)/Math.Max(U.Ę.Count,1));}if(U.Ę
!=null&&U.Ę.Count>0){U.Ĝ=U.Ę[0].CubeGrid;if(this.Ň.ұ){U.Ě=new Vector3I[4];Vector3I Ƴ=new Vector3I();Vector3I Ƹ=new
Vector3I();foreach(IMyUserControllableGun ƾ in U.Ę){Ƴ=Vector3I.Min(Ƴ,ƾ.Position);Ƹ=Vector3I.Max(Ƹ,ƾ.Position);}Vector3I Ǆ=
Base6Directions.GetIntVector(U.Ę[0].Orientation.Forward);if(Ǆ.X!=0){U.Ě[0]=new Vector3I(0,Ƴ.Y,Ƴ.Z);U.Ě[1]=new Vector3I(0,Ƴ.Y,Ƹ.Z);U.Ě[2
]=new Vector3I(0,Ƹ.Y,Ƴ.Z);U.Ě[3]=new Vector3I(0,Ƹ.Y,Ƹ.Z);}else if(Ǆ.Y!=0){U.Ě[0]=new Vector3I(Ƴ.X,0,Ƴ.Z);U.Ě[1]=new
Vector3I(Ƴ.X,0,Ƹ.Z);U.Ě[2]=new Vector3I(Ƹ.X,0,Ƴ.Z);U.Ě[3]=new Vector3I(Ƹ.X,0,Ƹ.Z);}else{U.Ě[0]=new Vector3I(Ƴ.X,Ƴ.Y,0);U.Ě[1]=
new Vector3I(Ƴ.X,Ƹ.Y,0);U.Ě[2]=new Vector3I(Ƹ.X,Ƴ.Y,0);U.Ě[3]=new Vector3I(Ƹ.X,Ƹ.Y,0);}}else{Vector3I ǃ=new Vector3I();
foreach(IMyUserControllableGun ƾ in U.Ę){ǃ+=ƾ.Position;}ǃ=new Vector3I((int)((float)ǃ.X/U.Ę.Count),(int)((float)ǃ.Y/U.Ę.Count),
(int)((float)ǃ.Z/U.Ę.Count));U.Ě=new Vector3I[]{ǃ};}}}if(Ņ.Length>0){ǀ(Ÿ,Ņ[0],ref ŷ);}}public void ǂ(Ż ǁ){ļ=ǁ.ļ;ņ=ǁ.ņ;Ĳ=ǁ
.Ĳ;Ŗ=ǁ.Ŗ;ŕ=ǁ.ŕ;Ŕ=ǁ.Ŕ;œ=ǁ.œ;Ŝ=ǁ.Ŝ;Œ=ǁ.Œ;Ő=ǁ.Ő;}public void ǀ(IMyMotorStator Ŏ,f Ä,ref double ƿ){IMyMotorStator U=Ä.e;
double É=Math.Cos(U.Angle);double Ʃ=Math.Sin(U.Angle);Vector3D ƙ=(U.WorldMatrix.Backward*É)+(U.WorldMatrix.Left*Ʃ);Vector3D Ơ=
(U.WorldMatrix.Left*É)+(U.WorldMatrix.Forward*Ʃ);double ƽ=Ä.Z+Ɣ(Ä.Û.WorldMatrix.Forward,ƙ,Ơ);if(ƽ>=MathHelperD.TwoPi)ƽ-=
MathHelperD.TwoPi;É=Math.Cos(ƽ);Ʃ=Math.Sin(ƽ);Vector3D Ƽ=(Ä.e.WorldMatrix.Backward*É)+(Ä.e.WorldMatrix.Left*Ʃ);É=Math.Cos(Ŏ.Angle);
Ʃ=Math.Sin(Ŏ.Angle);Vector3D ƻ=(Ŏ.WorldMatrix.Backward*É)+(Ŏ.WorldMatrix.Left*Ʃ);Vector3D ƺ=(Ŏ.WorldMatrix.Left*É)+(Ŏ.
WorldMatrix.Forward*Ʃ);ƿ=-Ɣ(Ƽ,ƻ,ƺ);ƿ=(Math.Round(ƿ/MathHelper.PiOver2)%4)*MathHelper.PiOver2;}public void ƹ(f U){IMyMotorStator C=U
.e;double É=Math.Cos(C.Angle);double Ʃ=Math.Sin(C.Angle);Vector3D ƙ=(C.WorldMatrix.Backward*É)+(C.WorldMatrix.Left*Ʃ);
Vector3D Ơ=(C.WorldMatrix.Left*É)+(C.WorldMatrix.Forward*Ʃ);U.Z=Ɣ(U.Û.WorldMatrix.Forward,ƙ,Ơ);double B=C.LowerLimitRad;double L
=C.UpperLimitRad;double Ƙ;if(C.LowerLimitDeg==float.MinValue||C.UpperLimitDeg==float.MaxValue){Ƙ=C.Angle+U.Z;}else{Ƙ=((L+
B)*0.5)+U.Z;}F(ref Ƙ);É=Math.Cos(Ƙ);Ʃ=Math.Sin(Ƙ);Vector3D Ɨ=(C.WorldMatrix.Backward*É)+(C.WorldMatrix.Left*Ʃ);Vector3D Ɩ
=C.WorldMatrix.Up.Cross(Ÿ.WorldMatrix.Up);U.S=(Ɩ.Dot(Ɨ)<0);double ƕ=Ɣ(C.WorldMatrix.Backward,U.S?-Ɩ:Ɩ,Ÿ.WorldMatrix.Up);
if(U.S)ƕ=MathHelper.TwoPi-ƕ;U.Z=-ƕ-U.Z;U.Z=(Math.Round(U.Z/MathHelper.PiOver2)%4)*MathHelper.PiOver2;}public double Ɣ(
Vector3D Ɠ,Vector3D ƒ,Vector3D Ƒ){double Ɛ=Math.Round(ƒ.Dot(Ɠ));if(Ɛ==0){if(Ƒ.Dot(Ɠ)>0)return MathHelperD.PiOver2;else return
MathHelperD.PiOver2+MathHelperD.Pi;}else{if(Ɛ>0)return 0;else return MathHelperD.Pi;}}public void Ə(){Ÿ.TargetVelocityRad=0f;for(
int Ǝ=0;Ǝ<Ņ.Length;Ǝ++){Ņ[Ǝ].e.TargetVelocityRad=0f;}}public bool ƍ(){return(Ÿ.IsWorking&&Ÿ.IsAttached);}public bool ƌ(f U)
{return(U.e.IsWorking&&U.e.IsAttached&&U.Û.IsFunctional);}public bool ƌ(){foreach(f U in Ņ){if(U.e.IsWorking&&U.e.
IsAttached&&U.Û.IsFunctional){return true;}}return false;}public f Ƌ(){foreach(f U in Ņ){if(U.e.IsWorking&&U.e.IsAttached&&U.Û.
IsFunctional){return U;}}return Ņ[0];}public bool Ɗ(Ū Á,int À,out Vector3D º,out double µ){Vector3D Ƨ=Ņ[0].Û.WorldMatrix.Translation
;Vector3D Ʀ;if(Á.ƀ!=null){Ʀ=Ƣ(Á,À);}else{Ʀ=Á.Ũ;}Vector3D ƥ=(Á.ŧ==À?Ʀ:Ʀ+((À-Á.ŧ)*ś*Á.ţ));Vector3D Ƥ=(Ł==null?Vector3D.Zero
:Ł.GetShipVelocities().LinearVelocity);if(ń.Ȗ!=0){Ƨ+=Ņ[0].Û.WorldMatrix.Forward*ń.Ȗ;}ƥ=ń.Ȋ(ref ƥ,ref Á.ţ,ref Ƨ,ref Ƥ,Á);
if(double.IsNaN(ƥ.Sum)){º=Vector3D.Zero;µ=0;return false;}else{º=ƥ-Ƨ;µ=º.Length();º=(µ==0?Vector3D.Zero:º/µ);return true;}
}public Vector3D Ƣ(Ū Á,int À){if(Á.ƀ?.Count==0||À>Á.ş+ř||Á.Ų==null){return Á.Ũ;}bool ƨ=false;if(Ŗ!=Á.ũ){Ŗ=Á.ũ;ŕ=0;Ŕ=
Vector3D.TransformNormal(Á.Ũ-Á.Ŧ,MatrixD.Transpose(Á.Ų.Value));œ=Á.ƀ[0].ʦ;ƨ=true;}else if(Ő>1){ŕ++;if(ŕ>=Á.ƀ.Count){ŕ=0;}Ŕ=œ;œ=Á
.ƀ[ŕ].ʦ;ƨ=true;}if(ƨ){Ŝ=À;double ơ=(Ŕ-œ).Length();if(ơ<1){Œ=ś;}else{Œ=ś/ơ*(ŗ*(Á.Ŧ-Ÿ.WorldMatrix.Translation).Length());}Ő
=0;}Ő+=(À-Ŝ)*Œ;Ŝ=À;return Á.Ŧ+Vector3D.TransformNormal(Vector3D.Lerp(Ŕ,œ,Ő),Á.Ų.Value);}private bool Ɵ(double w,double Ã,
int À,bool ƞ=false){w+=ŷ;if(w>=MathHelperD.TwoPi)w-=MathHelperD.TwoPi;double Ɲ;if(ž){if(!Ð(out Ɲ,w,Ş,Ŷ,ŵ)){return false;}}
else{Ɲ=w-Ş;if(Ɲ>Math.PI)Ɲ-=MathHelperD.TwoPi;else if(Ɲ<-Math.PI)Ɲ+=MathHelperD.TwoPi;}if(!ƞ){if(ő==null){Ÿ.TargetVelocityRad
=Math.Min(ł,Math.Max(-ł,(float)(Ɲ*Ň.һ)));P(Ÿ,Ɲ);}else{Ÿ.TargetVelocityRad=(float)ő.Ĭ(Ş,w,À);}}for(int Ǝ=0;Ǝ<Ņ.Length;Ǝ++)
{f Ɯ=Ņ[Ǝ];if(!ƌ(Ɯ))continue;double ƛ;ƛ=Ɯ.ð;double ƚ=Ã;if(Ɯ.S){ƛ=Ɯ.Z-ƛ;if(ƛ<-MathHelperD.TwoPi)ƛ+=MathHelperD.TwoPi;ƚ=Ɯ.Z-
ƚ;if(ƚ<-MathHelperD.TwoPi)ƚ+=MathHelperD.TwoPi;}else{ƛ+=Ɯ.Z;if(ƛ>=MathHelperD.TwoPi)ƛ-=MathHelperD.TwoPi;ƚ+=Ɯ.Z;if(ƚ>=
MathHelperD.TwoPi)ƚ-=MathHelperD.TwoPi;}double ı;if(Ɯ.W){if(!Ð(out ı,ƚ,ƛ,Ɯ.Y,Ɯ.X)){return false;}}else{ı=ƚ-ƛ;if(ı>Math.PI)ı-=
MathHelperD.TwoPi;else if(ı<-Math.PI)ı+=MathHelperD.TwoPi;}if(!ƞ){if(Ɯ.ę==null){Ɯ.e.TargetVelocityRad=Math.Min(ł,Math.Max(-ł,(float
)(ı*Ň.һ)));P(Ɯ.e,ı);}else{Ɯ.e.TargetVelocityRad=(float)Ɯ.ę.Ĭ(ƛ,ƚ,À);}}else{break;}}return true;}public bool Ç(){long V=
DateTime.Now.Ticks;if(ĺ==null||Ĺ==null){return false;}if((ĺ.GetInventory()?.IsItemAt(0)??false)&&(!Ĺ.GetInventory()?.IsItemAt(0)
??false)){return true;}return false;}public bool Æ(int À){if(ĺ==null||Ĺ==null){return true;}if(Ļ){ĵ=Ē.đ;return true;}else
if(!ƍ()||!ƌ()){ĵ=Ē.đ;Ļ=true;return true;}if(ĵ==Ē.đ){o(true);ĵ=Ē.Đ;}f Ä=Ƌ();double Ã;switch(ĵ){case Ē.Đ:Ò(Ņ,Ä,Ÿ);Ã=Ĵ;if(
Math.Abs(Ã-Ä.ð)>0.0018){Ɵ(Ş,Ã,À,false);}else{ĵ=Ē.ď;}break;case Ē.ď:Ò(Ņ,Ä,Ÿ);Ã=ĳ;if(Math.Abs(Ã-Ä.ð)>0.0018){Ɵ(Ş,Ã,À,false);}
else{ĵ=Ē.Ď;}break;case Ē.Ď:if(Ĺ.Status==MyShipConnectorStatus.Connected){ĵ=Ē.č;}else if(Ĺ.Status==MyShipConnectorStatus.
Connectable){if(À>=ĸ+30){Ĺ.Connect();ĸ=À;}}break;case Ē.č:if(ĺ.GetInventory()?.IsItemAt(0)??false){Ĺ.GetInventory()?.
TransferItemFrom(ĺ.GetInventory(),0,0,true);}Ĺ.Disconnect();ĺ.Disconnect();ĵ=Ē.đ;return true;}return false;}public bool Å(Ū Á,int À){if(
Ļ){return false;}else if(!ƍ()||!ƌ()){Ļ=true;return false;}if(ĵ!=Ē.đ){return false;}Vector3D º;double µ;bool ª=Ɗ(Á,À,out º
,out µ);if(ª){f Ä=Ƌ();if(ž||Ä.W){double w,Ã;Ò(Ņ,Ä,Ÿ);Ï(º,Ÿ,out w,out Ã);if(!Ɵ(w,Ã,À,true)){return false;}}if(ľ!=null&&ľ.ո
(Ä.Û.WorldMatrix.Translation,º)){return false;}if(µ<=ń.ȗ){return true;}}return false;}public bool Â(Ū Á,int À){if(Ļ){o(
true);Ə();return false;}else if(!ƍ()||!ƌ()){o(true);Ə();Ļ=true;return false;}if(ĵ!=Ē.đ){return false;}Vector3D º;double µ;
bool ª=Ɗ(Á,À,out º,out µ);if(ª){f Ä=Ƌ();double w,Ã;Ò(Ņ,Ä,Ÿ);Ï(º,Ÿ,out w,out Ã);if(Ɵ(w,Ã,À,false)){if(µ<=ń.ȗ){bool Ú=false;
foreach(f U in Ņ){if(ľ!=null&&U.Ĝ!=null&&U.Ě!=null){bool Ù=false;for(int Ø=0;Ø<U.Ě.Length;Ø++){if(ľ.ո(m(U.Ĝ,ref U.Ě[Ø]),º)){Ù=
true;break;}}if(Ù){o(U);continue;}}double Ö=U.Û.WorldMatrix.Forward.Dot(º);bool Õ=(Ö>=Ň.Ӂ);if(!Õ&&Ö>=Ř&&ņ==Á.ũ){double Ô=U.Û
.WorldMatrix.Forward.Dot(Ĳ);double Ó=º.Dot(Ĳ);Õ=(Ô>=Ň.Ӂ)||(Ö>=Ó&&Ô>=Ó);}if(Õ){A(U,À,ń.ȓ);Ú=true;}else{o(U);}}if(Ú){Á.ƃ+=Ľ
;}ņ=Á.ũ;Ĳ=º;return true;}}}o();Ə();return false;}public void Ò(f[]Ñ,f Ä,IMyMotorStator Í){Ş=Í.Angle;F(ref Ş);foreach(f U
in Ñ){if(U==Ä||ƌ(U)){double Ê=U.Û.WorldMatrix.Forward.Dot(Í.WorldMatrix.Up);U.ð=MathHelperD.PiOver2-Math.Acos(MathHelper.
Clamp(Ê,-1,1));}}}public void Ï(Vector3D Î,IMyMotorStator Í,out double Ì,out double Ë){double Ê=Î.Dot(Í.WorldMatrix.Up);Ë=
MathHelperD.PiOver2-Math.Acos(MathHelper.Clamp(Ê,-1,1));double É=Math.Cos(Ë);Vector3D È=(É==0?Vector3D.Zero:(Î-(Í.WorldMatrix.Up*Ê)
)/É);Ì=Math.Acos(MathHelper.Clamp(Í.WorldMatrix.Backward.Dot(È),-1,1));if(Í.WorldMatrix.Right.Dot(È)>0)Ì=MathHelperD.
TwoPi-Ì;}public bool Ð(out double v,double N,double R,double B,double L){L-=B;N-=B;R-=B;F(ref L);F(ref N);F(ref R);if(N>=0&&N
<=L){if(R>L){R=(R-L<=MathHelperD.TwoPi-R?L:0);}v=N-R;return true;}else{v=0;return false;}}public void P(IMyMotorStator C,
double O){float N=C.Angle+(float)O;if(N<-MathHelper.TwoPi||N>MathHelper.TwoPi){C.SetValueFloat("LowerLimit",float.MinValue);C.
SetValueFloat("UpperLimit",float.MaxValue);}else if(O<0){C.UpperLimitRad=C.Angle;C.LowerLimitRad=N;}else if(O>0){C.LowerLimitRad=C.
Angle;C.UpperLimitRad=N;}}public bool M(IMyMotorStator C,out double B,out double L){double K=double.MinValue;double H=double.
MaxValue;if(Ň.ҿ){MyIni G=new MyIni();G.TryParse(C.CustomData);K=G.Get(Ś,"LowerLimit").ToDouble(K);H=G.Get(Ś,"UpperLimit").
ToDouble(H);if(K==double.MinValue||H==double.MaxValue){K=MathHelper.Clamp(Math.Round(C.LowerLimitDeg,3),-361,361);H=MathHelper.
Clamp(Math.Round(C.UpperLimitDeg,3),-361,361);G.Set(Ś,"LowerLimit",K);G.Set(Ś,"UpperLimit",H);C.CustomData=G.ToString();}}
else{K=MathHelper.Clamp(Math.Round(C.LowerLimitDeg,3),-361,361);H=MathHelper.Clamp(Math.Round(C.UpperLimitDeg,3),-361,361);}
if(K<-360)K=double.MinValue;if(H>360)H=double.MaxValue;if(H<K||H-K>=360){K=double.MinValue;H=double.MaxValue;}if(K>double.
MinValue&&H<double.MaxValue){B=MathHelperD.ToRadians(K);L=MathHelperD.ToRadians(H);return true;}else{B=double.MinValue;L=double.
MaxValue;return false;}}public void F(ref double E){if(E<0){if(E<=-MathHelperD.TwoPi)E+=MathHelperD.FourPi;else E+=MathHelperD.
TwoPi;}else if(E>=MathHelperD.TwoPi){if(E>=MathHelperD.FourPi)E-=MathHelperD.FourPi;else E-=MathHelperD.TwoPi;}}public void D
(IMyMotorStator C,double B,double L){if(B==double.MinValue)C.SetValueFloat("LowerLimit",float.MinValue);else C.
LowerLimitRad=(float)B;if(L==double.MaxValue)C.SetValueFloat("UpperLimit",float.MaxValue);else C.UpperLimitRad=(float)L;}public void
A(f U,int s,bool r){if(r){if(U.Ę.Count>0&&U.Ĕ<=s){if(U.ĕ>=U.Ę.Count){U.ĕ=0;}IMyUserControllableGun q=U.Ę[U.ĕ];if(Ň.Ӫ!=
null){Ň.Ӫ.ǚ(q,true,true);U.Ė=true;}else Ŀ.Apply(q);U.Ĕ=s+U.ē;U.ĕ++;}}else{if(!U.ė){foreach(IMyUserControllableGun q in U.Ę){
if(Ň.Ӫ!=null)Ň.Ӫ.ǚ(q,true,true);else ŀ.SetValue(q,true);}U.ė=true;}}}public void o(bool n=false){foreach(f U in Ņ){o(U,n);
}}public void o(f U,bool n=false){if(U.ė||U.Ė||n){foreach(IMyUserControllableGun q in U.Ę){if(Ň.Ӫ!=null)Ň.Ӫ.ǚ(q,false,
true);else ŀ.SetValue(q,false);}U.ė=false;U.Ė=false;}}public Vector3D m(IMyCubeGrid l,ref Vector3I h){MatrixD g=l.
WorldMatrix;return(g.Translation+(g.Right*h.X)+(g.Up*h.Y)+(g.Backward*h.Z));}public class f{public IMyMotorStator e;public double Z
;public double Y;public double X;public bool W;public bool S;public IMyTerminalBlock Û;public double ð;public IMyCubeGrid
Ĝ;public Vector3I[]Ě;public Č ę;public List<IMyUserControllableGun>Ę;public bool ė;public bool Ė;public int ĕ;public int
Ĕ;public int ē;}public enum Ē{đ,Đ,ď,Ď,č}}public class Č{const int ċ=3;double Ċ;double ĉ;double Ĉ;double ě;double ć;int ĝ;
public Č(double į,double Į,double ĭ){Ċ=į;ĉ=Į;Ĉ=ĭ;}public double Ĭ(double ï,double ī,int s){int Ī=Math.Max(s-ĝ,1);double ĩ=ī-ć;
if(Ī<ċ){ĩ*=(double)ċ/Ī;Ī=ċ;}Ĩ(ref ĩ);if(ě*ĩ<0){ě=(ĉ*ĩ);}else{ě=((1-ĉ)*ě)+(ĉ*ĩ);}double İ=ī-ï+ě;Ĩ(ref İ);ć=ī;ĝ=Math.Max(s,ĝ
);return MathHelper.Clamp(İ*Ċ/Ī,-Ĉ,Ĉ);}public void Ĩ(ref double E){if(E<-Math.PI){E+=MathHelperD.TwoPi;if(E<-Math.PI)E+=
MathHelperD.TwoPi;}else if(E>Math.PI){E-=MathHelperD.TwoPi;if(E>Math.PI)E-=MathHelperD.TwoPi;}}public void Ħ(){ĝ=0;ě=ć=0;}}class ĥ{
public int Ĥ;public double ģ;public double Ģ;public double ġ;public double Ġ;public double ğ;public IMyGridProgramRuntimeInfo
Ğ{get;private set;}public Queue<double>ħ=new Queue<double>();public Queue<double>Ć=new Queue<double>();public Dictionary<
string,ă>û=new Dictionary<string,ă>();private double ì;private double é;public ĥ(IMyGridProgramRuntimeInfo Ü,int è,double ç){Ğ
=Ü;Ĥ=è;ģ=ç;ì=6;é=100.0/(Ğ.MaxInstructionCount==0?50000:Ğ.MaxInstructionCount);}public void æ(){Ģ=0;ħ.Clear();ġ=0;Ġ=0;Ć.
Clear();ğ=0;}public void å(){double Ü=Ğ.LastRunTimeMs;Ģ+=(Ü-Ģ)*ģ;ħ.Enqueue(Ü);if(ħ.Count>Ĥ)ħ.Dequeue();ġ=ħ.Max();}public void
ä(){double ã=Ğ.CurrentInstructionCount;Ġ+=(ã-Ġ)*ģ;Ć.Enqueue(ã);if(Ć.Count>Ĥ)Ć.Dequeue();ğ=Ć.Max();}public void â(
StringBuilder á){á.AppendLine($"Avg Runtime = {Ģ:0.0000}ms   ({Ģ*ì:0.00}%)");á.AppendLine($"Peak Runtime = {ġ:0.0000}ms\n");á.
AppendLine($"Avg Complexity = {Ġ:0.00}   ({Ġ*é:0.00}%)");á.AppendLine($"Peak Complexity = {ğ:0.00}");}public void à(string Þ){ă Ý;
if(û.ContainsKey(Þ)){Ý=û[Þ];}else{Ý=new ă();û[Þ]=Ý;}Ý.Ā=DateTime.Now.Ticks;}public void ß(string Þ){ă Ý;if(û.TryGetValue(Þ
,out Ý)){long ï=DateTime.Now.Ticks;double Ü=(ï-Ý.Ā)*0.0001;Ý.Ă++;Ý.ā+=Ü;Ý.Ā=ï;}}public void ñ(StringBuilder á){foreach(
KeyValuePair<string,ă>Ą in û){double Ü=(Ą.Value.Ă==0?0:Ą.Value.ā/Ą.Value.Ă);á.AppendLine($"{Ą.Key} = {Ü:0.0000}ms");}}public class ă
{public long Ă;public double ā;public long Ā;}}class ÿ{float þ;double ý;bool ą;List<IMyCameraBlock>ü;public List<
IMyCameraBlock>ú{get{return ü;}set{foreach(IMyCameraBlock ô in value){ô.Enabled=true;ô.EnableRaycast=true;}ü=value;ö();}}List<ɯ>ù;
public ÿ(List<IMyCameraBlock>ø){if(ø.Count>0){þ=ø[0].RaycastConeLimit;if(þ==0f||þ==180f)ý=double.NaN;else ý=Math.Tan(
MathHelper.ToRadians(90-þ));ą=double.IsNaN(ý)||double.IsInfinity(ý);if(ą)ý=1;}else{þ=45;ý=1;ą=false;}ú=ø;MyMath.InitializeFastSin(
);}private void ö(){if(þ<=0||þ>=180){ù=new List<ɯ>();return;}Dictionary<string,ɯ>õ=new Dictionary<string,ɯ>();foreach(
IMyCameraBlock ô in ü){string ó=ô.CubeGrid.EntityId.ToString()+"-"+((int)ô.Orientation.Forward).ToString();ɯ ƪ;if(õ.ContainsKey(ó)){ƪ=
õ[ó];}else{ƪ=new ɯ();ƪ.ú=new List<IMyCameraBlock>();ƪ.ɗ=ý;ƪ.ɖ=ą;õ[ó]=ƪ;}ƪ.ú.Add(ô);}ù=õ.Values.ToList();foreach(ɯ ƪ in ù)
{ƪ.ɕ=ƪ.ú[0].CubeGrid;int ɪ=int.MaxValue,ɩ=int.MinValue,ɨ=int.MaxValue,ɧ=int.MinValue,ɦ=int.MaxValue,ɻ=int.MinValue;
foreach(IMyCameraBlock ô in ƪ.ú){ɪ=Math.Min(ɪ,ô.Position.X);ɩ=Math.Max(ɩ,ô.Position.X);ɨ=Math.Min(ɨ,ô.Position.Y);ɧ=Math.Max(ɧ,
ô.Position.Y);ɦ=Math.Min(ɦ,ô.Position.Z);ɻ=Math.Max(ɻ,ô.Position.Z);}Base6Directions.Direction ɺ=ƪ.ɕ.WorldMatrix.
GetClosestDirection(ƪ.ú[0].WorldMatrix.Up);Base6Directions.Direction ɹ=ƪ.ɕ.WorldMatrix.GetClosestDirection(ƪ.ú[0].WorldMatrix.Left);
Base6Directions.Direction ɸ=ƪ.ɕ.WorldMatrix.GetClosestDirection(ƪ.ú[0].WorldMatrix.Forward);ƪ.ɏ=ɯ.ɭ(ɺ);ƪ.Ɏ=ɯ.ɭ(ɹ);ƪ.ɍ=ɯ.ɭ(ɸ);ƪ.ɔ=ɯ.ɬ(ɺ,
ɪ,ɩ,ɨ,ɧ,ɦ,ɻ);ƪ.ɓ=ɯ.ɬ(Base6Directions.GetOppositeDirection(ɺ),ɪ,ɩ,ɨ,ɧ,ɦ,ɻ);ƪ.ɑ=ɯ.ɬ(ɹ,ɪ,ɩ,ɨ,ɧ,ɦ,ɻ);ƪ.ɐ=ɯ.ɬ(Base6Directions.
GetOppositeDirection(ɹ),ɪ,ɩ,ɨ,ɧ,ɦ,ɻ);}}public bool ɷ(ref Vector3D ƥ,out MyDetectedEntityInfo ɶ,double ɵ=0){IMyCameraBlock ô=ɲ(ref ƥ);if(ô!=
null){if(ɵ==0){ɶ=ɷ(ô,ref ƥ);}else{Vector3D ɴ=ƥ-ô.WorldMatrix.Translation;Vector3D ɳ=ƥ+((ɵ/Math.Max(ɴ.Length(),
0.000000000000001))*ɴ);ɶ=ɷ(ô,ref ɳ);}return true;}else{ɶ=default(MyDetectedEntityInfo);return false;}}IMyCameraBlock ɲ(ref Vector3D ƥ){
foreach(ɯ ɱ in ù){if(ɱ.ɣ(ref ƥ)){return ɰ(ɱ,ref ƥ);}}return null;}IMyCameraBlock ɰ(ɯ ɼ,ref Vector3D ƥ){bool ɽ=true;for(int Ǝ=0;
Ǝ<ɼ.ú.Count;Ǝ++){if(ɼ.ɘ>=ɼ.ú.Count){ɼ.ɘ=0;}IMyCameraBlock ô=ɼ.ú[ɼ.ɘ++];if(ô.IsWorking){if(ʄ(ô,ref ƥ)){return ô;}else if(ɽ
){ɽ=false;if(!ɼ.ɣ(ref ƥ)){break;}}}}return null;}bool ʄ(IMyCameraBlock ô,ref Vector3D ɢ){Vector3D ɡ=(ą?Vector3D.Zero:ô.
WorldMatrix.Forward);Vector3D ɠ=ô.WorldMatrix.Left;Vector3D ɟ=ô.WorldMatrix.Up;Vector3D Ŭ=ɢ-ô.WorldMatrix.Translation;if(ý>=0){
return(ô.AvailableScanRange*ô.AvailableScanRange>=Ŭ.LengthSquared())&&Ŭ.Dot(ɡ+ɠ)>=0&&Ŭ.Dot(ɡ-ɠ)>=0&&Ŭ.Dot(ɡ+ɟ)>=0&&Ŭ.Dot(ɡ-ɟ)
>=0;}else{return(ô.AvailableScanRange*ô.AvailableScanRange>=Ŭ.LengthSquared())&&(Ŭ.Dot(ɡ+ɠ)>=0||Ŭ.Dot(ɡ-ɠ)>=0||Ŭ.Dot(ɡ+ɟ)
>=0||Ŭ.Dot(ɡ-ɟ)>=0);}}void ʃ(IMyCameraBlock ô,ref Vector3D ɢ,out double ʁ,out double ı,out double Ɲ){Vector3D º=ɢ-ô.
WorldMatrix.Translation;º=Vector3D.TransformNormal(º,MatrixD.Transpose(ô.WorldMatrix));Vector3D ʀ=Vector3D.Normalize(new Vector3D(º
.X,0,º.Z));ʁ=º.Normalize();Ɲ=MathHelper.ToDegrees(Math.Acos(MathHelper.Clamp(ʀ.Dot(Vector3D.Forward),-1,1))*Math.Sign(º.X
));ı=MathHelper.ToDegrees(Math.Acos(MathHelper.Clamp(ʀ.Dot(º),-1,1))*Math.Sign(º.Y));}MyDetectedEntityInfo ɷ(
IMyCameraBlock ô,ref Vector3D ɢ){double ɿ,ɾ,ʂ;ʃ(ô,ref ɢ,out ɿ,out ɾ,out ʂ);return ô.Raycast(ɿ,(float)ɾ,(float)ʂ);}public class ɯ{
public List<IMyCameraBlock>ú;public int ɘ;public double ɗ;public bool ɖ;public IMyCubeGrid ɕ;public Vector3I ɔ;public Vector3I
ɓ;public Vector3I ɑ;public Vector3I ɐ;public Func<IMyCubeGrid,Vector3D>ɏ;public Func<IMyCubeGrid,Vector3D>Ɏ;public Func<
IMyCubeGrid,Vector3D>ɍ;public static Vector3D Ɍ(IMyCubeGrid l){return l.WorldMatrix.Up;}public static Vector3D ɋ(IMyCubeGrid l){
return l.WorldMatrix.Down;}public static Vector3D Ɋ(IMyCubeGrid l){return l.WorldMatrix.Left;}public static Vector3D Ɉ(
IMyCubeGrid l){return l.WorldMatrix.Right;}public static Vector3D ə(IMyCubeGrid l){return l.WorldMatrix.Forward;}public static
Vector3D ɛ(IMyCubeGrid l){return l.WorldMatrix.Backward;}public static Func<IMyCubeGrid,Vector3D>ɭ(Base6Directions.Direction ɫ){
switch(ɫ){case Base6Directions.Direction.Up:return Ɍ;case Base6Directions.Direction.Down:return ɋ;case Base6Directions.
Direction.Left:return Ɋ;case Base6Directions.Direction.Right:return Ɉ;case Base6Directions.Direction.Forward:return ə;case
Base6Directions.Direction.Backward:return ɛ;default:return ə;}}public static Vector3I ɬ(Base6Directions.Direction ɫ,int ɪ,int ɩ,int ɨ,
int ɧ,int ɦ,int ɮ){switch(ɫ){case Base6Directions.Direction.Up:return new Vector3I((ɪ+ɩ)/2,ɧ,(ɦ+ɮ)/2);case Base6Directions.
Direction.Down:return new Vector3I((ɪ+ɩ)/2,ɨ,(ɦ+ɮ)/2);case Base6Directions.Direction.Left:return new Vector3I(ɪ,(ɨ+ɧ)/2,(ɦ+ɮ)/2);
case Base6Directions.Direction.Right:return new Vector3I(ɩ,(ɨ+ɧ)/2,(ɦ+ɮ)/2);case Base6Directions.Direction.Forward:return
new Vector3I((ɪ+ɩ)/2,(ɨ+ɧ)/2,ɦ);case Base6Directions.Direction.Backward:return new Vector3I((ɪ+ɩ)/2,(ɨ+ɧ)/2,ɮ);default:
return new Vector3I((ɪ+ɩ)/2,(ɨ+ɧ)/2,ɦ);}}Vector3D ɥ(ref Vector3D ɢ,ref Vector3I ɤ){return ɢ-ɕ.GridIntegerToWorld(ɤ);}public
bool ɣ(ref Vector3D ɢ){Vector3D ɡ=(ɖ?Vector3D.Zero:ɍ(ɕ));Vector3D ɠ=ɗ*Ɏ(ɕ);Vector3D ɟ=ɗ*ɏ(ɕ);if(ɗ>=0){return(ɥ(ref ɢ,ref ɐ).
Dot(ɡ+ɠ)>=0&&ɥ(ref ɢ,ref ɑ).Dot(ɡ-ɠ)>=0&&ɥ(ref ɢ,ref ɓ).Dot(ɡ+ɟ)>=0&&ɥ(ref ɢ,ref ɔ).Dot(ɡ-ɟ)>=0);}else{return(ɥ(ref ɢ,ref ɐ
).Dot(ɡ+ɠ)>=0||ɥ(ref ɢ,ref ɑ).Dot(ɡ-ɠ)>=0||ɥ(ref ɢ,ref ɓ).Dot(ɡ+ɟ)>=0||ɥ(ref ɢ,ref ɔ).Dot(ɡ-ɟ)>=0);}}}}class ɞ<ɝ>{private
List<ɝ>ɚ;private Func<ɝ,bool>ɜ;private int ʅ;private int ï;private bool ʮ;public ɞ(List<ɝ>ʨ,Func<ɝ,bool>ʭ=null){ɚ=ʨ;ɜ=ʭ;ʅ=ï=
0;ʮ=false;if(ɚ==null)ɚ=new List<ɝ>();}public void Ħ(){ʅ=ï=0;}public void ʬ(){ʅ=ï;ʮ=(ɚ.Count>0);}public ɝ ʫ(){if(ʅ>=ɚ.
Count)ʅ=0;if(ï>=ɚ.Count){ï=0;ʮ=(ɚ.Count>0);}ɝ ʪ=default(ɝ);while(ʮ){ɝ ʩ=ɚ[ï++];if(ï>=ɚ.Count)ï=0;if(ï==ʅ)ʮ=false;if(ɜ==null||
ɜ(ʩ)){ʪ=ʩ;break;}}return ʪ;}public void ʯ(List<ɝ>ʨ){ɚ=ʨ;if(ɚ==null)ɚ=new List<ɝ>();if(ʅ>=ɚ.Count)ʅ=0;if(ï>=ɚ.Count)ï=0;ʮ=
false;}}public struct ʧ{public Vector3D ʦ;public int ʥ;public ʧ(ref Vector3D ʤ,int À){ʦ=ʤ;ʥ=À;}}public class ʣ{public long ũ;
public Vector3D Ũ;public Vector3D ţ;}class ʰ{const int ŝ=Program.ŝ;Dictionary<long,ʜ>ʷ;SortedSet<ʜ>ʶ;HashSet<long>ʵ;public ʰ()
{ʷ=new Dictionary<long,ʜ>();ʶ=new SortedSet<ʜ>(new ʘ());ʵ=new HashSet<long>();}public bool ʳ(ref MyDetectedEntityInfo ɶ,
int s){Ū ʴ;return ʳ(ref ɶ,s,out ʴ);}public bool ʳ(ref MyDetectedEntityInfo ɶ,int s,out Ū Á){if(ʵ.Contains(ɶ.EntityId)){Á=
null;return false;}if(ʷ.ContainsKey(ɶ.EntityId)){ʜ ʌ=ʷ[ɶ.EntityId];Á=ʌ.ʙ;int ʑ=Math.Max(s-Á.ŧ,1);double ʛ=1.0/ʑ;Á.š=Á.ţ;Á.Š=
Á.ŧ;if(ɶ.HitPosition!=null){Á.Ũ=ɶ.HitPosition.Value;Á.Ŧ=ɶ.Position;Á.ť=s;Á.Ų=ɶ.Orientation;Á.ş=s;}else{Á.Ũ=ɶ.Position;}Á.
Ŵ=(ɶ.Type==MyDetectedEntityType.LargeGrid);Á.ţ=ɶ.Velocity;Á.ŧ=s;Vector3D ȑ=(Á.ţ-Á.š)*ʛ*ŝ;if(ȑ.LengthSquared()>1){Á.Ţ=(Á.Ţ
*0.25)+(ȑ*0.75);}else{Á.Ţ=Vector3D.Zero;}ʌ.ʚ=s;Á.Ɖ=ɶ.BoundingBox.Extents.LengthSquared()*0.5;return false;}else{Á=new Ū(ɶ
.EntityId);Á.Ũ=ɶ.Position;Á.ţ=Á.š=ɶ.Velocity;Á.ŧ=Á.Š=s;ʜ ʌ=new ʜ();ʌ.ũ=Á.ũ;ʌ.ʙ=Á;ʌ.ʚ=s;ʷ.Add(ɶ.EntityId,ʌ);Á.Ɖ=ɶ.
BoundingBox.Extents.LengthSquared()*0.5;ʶ.Add(ʌ);return true;}}public bool ʳ(ʣ ʲ,int s,bool ʱ=true){if(ʵ.Contains(ʲ.ũ)){return
false;}if(ʷ.ContainsKey(ʲ.ũ)){ʜ ʌ=ʷ[ʲ.ũ];Ū Á=ʌ.ʙ;int ʑ=Math.Max(s-Á.ŧ,1);double ʛ=1.0/ʑ;Á.š=Á.ţ;Á.Š=Á.ŧ;Á.Ũ=ʲ.Ũ;Á.ţ=ʲ.ţ;Á.ŧ=s
;Vector3D ȑ=(Á.ţ-Á.š)*ʛ*ŝ;if(ȑ.LengthSquared()>1){Á.Ţ=(Á.Ţ*0.25)+(ȑ*0.75);}else{Á.Ţ=Vector3D.Zero;}if(ʱ){ʌ.ʚ=s;}return
false;}else{Ū Á=new Ū(ʲ.ũ);Á.Ũ=ʲ.Ũ;Á.ţ=Á.š=ʲ.ţ;Á.ŧ=Á.Š=s;ʜ ʌ=new ʜ();ʌ.ũ=Á.ũ;ʌ.ʙ=Á;if(ʱ){ʌ.ʚ=s;}ʷ.Add(ʲ.ũ,ʌ);ʶ.Add(ʌ);return
true;}}public void ʐ(long ƈ,int s){if(ʷ.ContainsKey(ƈ)){ʜ ʌ=ʷ[ƈ];ʌ.ʙ.Ť=s;ʶ.Remove(ʌ);ʌ.Ť=s;ʶ.Add(ʌ);}}public bool ʏ(long ƈ){
return ʷ.ContainsKey(ƈ);}public bool ʎ(long ƈ,int ʍ){if(ʷ.ContainsKey(ƈ)){ʜ ʌ=ʷ[ƈ];return(ʌ.ʚ>=ʍ);}else{return false;}}public
int ʋ(){return ʷ.Count;}public Ū ʊ(long ƈ){ʜ ʉ;if(ʷ.TryGetValue(ƈ,out ʉ))return ʉ.ʙ;else return null;}public List<Ū>ʈ(){
List<Ū>ʇ=new List<Ū>(ʷ.Count);foreach(ʜ ʌ in ʶ){ʇ.Add(ʌ.ʙ);}return ʇ;}public Ū ʆ(){if(ʶ.Count==0)return null;else return ʶ.
Min.ʙ;}public Ū ʒ(){double ʢ=double.MinValue;Ū ʡ=null;foreach(ʜ ʌ in ʶ){if(ʌ.ʙ.Ɖ>ʢ){ʢ=ʌ.ʙ.Ɖ;ʡ=ʌ.ʙ;}}return ʡ;}public void ʠ
(long ƈ){if(ʷ.ContainsKey(ƈ)){ʶ.Remove(ʷ[ƈ]);ʷ.Remove(ƈ);}}public void ʟ(){ʷ.Clear();ʶ.Clear();}public void ʞ(long ƈ){ʵ.
Add(ƈ);}public void ʝ(){ʵ.Clear();}class ʜ{public long ũ;public int Ť;public int ʚ;public Ū ʙ;}class ʘ:IComparer<ʜ>{public
int Compare(ʜ Ž,ʜ ż){if(Ž==null)return(ż==null?0:1);else if(ż==null)return-1;else return(Ž.Ť<ż.Ť?-1:(Ž.Ť>ż.Ť?1:(Ž.ũ<ż.ũ?-1:
(Ž.ũ>ż.ũ?1:0))));}}}public class ʗ{private Action<ICollection<MyDefinitionId>>ʖ;private Action<ICollection<MyDefinitionId
>>ʕ;private Action<ICollection<MyDefinitionId>>ʔ;private Func<Sandbox.ModAPI.Ingame.IMyTerminalBlock,IDictionary<string,
int>,bool>ʓ;private Func<long,MyTuple<bool,int,int>>Ɇ;private Action<Sandbox.ModAPI.Ingame.IMyTerminalBlock,IDictionary<
Sandbox.ModAPI.Ingame.MyDetectedEntityInfo,float>>ǲ;private Func<long,int,Sandbox.ModAPI.Ingame.MyDetectedEntityInfo>Ǔ;private
Func<Sandbox.ModAPI.Ingame.IMyTerminalBlock,long,int,bool>ǰ;private Func<Sandbox.ModAPI.Ingame.IMyTerminalBlock,int,Sandbox.
ModAPI.Ingame.MyDetectedEntityInfo>ǯ;private Action<Sandbox.ModAPI.Ingame.IMyTerminalBlock,long,int>Ǯ;private Action<Sandbox.
ModAPI.Ingame.IMyTerminalBlock,bool,int>ǭ;private Action<Sandbox.ModAPI.Ingame.IMyTerminalBlock,bool,bool,int>Ǭ;private Func<
Sandbox.ModAPI.Ingame.IMyTerminalBlock,int,bool,bool,bool>ǫ;private Func<Sandbox.ModAPI.Ingame.IMyTerminalBlock,int,float>Ǫ;
private Func<Sandbox.ModAPI.Ingame.IMyTerminalBlock,ICollection<string>,int,bool>ǩ;private Action<Sandbox.ModAPI.Ingame.
IMyTerminalBlock,ICollection<string>,int>Ǩ;private Action<Sandbox.ModAPI.Ingame.IMyTerminalBlock,float>ǧ;private Func<Sandbox.ModAPI.
Ingame.IMyTerminalBlock,long,int,bool>Ǧ;private Func<Sandbox.ModAPI.Ingame.IMyTerminalBlock,long,int,bool>ǥ;private Func<
Sandbox.ModAPI.Ingame.IMyTerminalBlock,long,int,Vector3D?>Ǥ;private Func<Sandbox.ModAPI.Ingame.IMyTerminalBlock,float>ǣ;private
Func<Sandbox.ModAPI.Ingame.IMyTerminalBlock,float>Ǣ;private Func<MyDefinitionId,float>ǡ;private Func<long,bool>Ǡ;private
Func<Sandbox.ModAPI.Ingame.IMyTerminalBlock,bool>ǟ;private Func<long,float>Ǟ;private Func<Sandbox.ModAPI.Ingame.
IMyTerminalBlock,int,string>Ǳ;private Action<Sandbox.ModAPI.Ingame.IMyTerminalBlock,int,string>ǝ;private Action<Action<Vector3,float>>ǳ;
private Action<Action<Vector3,float>>Ȇ;private Func<long,float>Ȅ;private Func<Sandbox.ModAPI.Ingame.IMyTerminalBlock,long>ȃ;
private Func<Sandbox.ModAPI.Ingame.IMyTerminalBlock,int,Matrix>Ȃ;private Func<Sandbox.ModAPI.Ingame.IMyTerminalBlock,int,Matrix
>ȁ;public bool Ȁ(Sandbox.ModAPI.Ingame.IMyTerminalBlock ǿ){var Ǿ=ǿ.GetProperty("WcPbAPI")?.As<Dictionary<string,Delegate>
>().GetValue(ǿ);return ǽ(Ǿ);}public bool ǽ(IReadOnlyDictionary<string,Delegate>Ǻ){if(Ǻ==null)return false;ȅ(Ǻ,
"GetCoreWeapons",ref ʖ);ȅ(Ǻ,"GetCoreStaticLaunchers",ref ʕ);ȅ(Ǻ,"GetCoreTurrets",ref ʔ);ȅ(Ǻ,"GetBlockWeaponMap",ref ʓ);ȅ(Ǻ,
"GetProjectilesLockedOn",ref Ɇ);ȅ(Ǻ,"GetSortedThreats",ref ǲ);ȅ(Ǻ,"GetAiFocus",ref Ǔ);ȅ(Ǻ,"SetAiFocus",ref ǰ);ȅ(Ǻ,"GetWeaponTarget",ref ǯ);ȅ(Ǻ,
"SetWeaponTarget",ref Ǯ);ȅ(Ǻ,"FireWeaponOnce",ref ǭ);ȅ(Ǻ,"ToggleWeaponFire",ref Ǭ);ȅ(Ǻ,"IsWeaponReadyToFire",ref ǫ);ȅ(Ǻ,
"GetMaxWeaponRange",ref Ǫ);ȅ(Ǻ,"GetTurretTargetTypes",ref ǩ);ȅ(Ǻ,"SetTurretTargetTypes",ref Ǩ);ȅ(Ǻ,"SetBlockTrackingRange",ref ǧ);ȅ(Ǻ,
"IsTargetAligned",ref Ǧ);ȅ(Ǻ,"CanShootTarget",ref ǥ);ȅ(Ǻ,"GetPredictedTargetPosition",ref Ǥ);ȅ(Ǻ,"GetHeatLevel",ref ǣ);ȅ(Ǻ,
"GetCurrentPower",ref Ǣ);ȅ(Ǻ,"GetMaxPower",ref ǡ);ȅ(Ǻ,"HasGridAi",ref Ǡ);ȅ(Ǻ,"HasCoreWeapon",ref ǟ);ȅ(Ǻ,"GetOptimalDps",ref Ǟ);ȅ(Ǻ,
"GetActiveAmmo",ref Ǳ);ȅ(Ǻ,"SetActiveAmmo",ref ǝ);ȅ(Ǻ,"RegisterProjectileAdded",ref ǳ);ȅ(Ǻ,"UnRegisterProjectileAdded",ref Ȇ);ȅ(Ǻ,
"GetConstructEffectiveDps",ref Ȅ);ȅ(Ǻ,"GetPlayerController",ref ȃ);ȅ(Ǻ,"GetWeaponAzimuthMatrix",ref Ȃ);ȅ(Ǻ,"GetWeaponElevationMatrix",ref ȁ);
return true;}private void ȅ<Ǽ>(IReadOnlyDictionary<string,Delegate>Ǻ,string ǹ,ref Ǽ Ǹ)where Ǽ:class{if(Ǻ==null){Ǹ=null;return;
}Delegate Ƿ;if(!Ǻ.TryGetValue(ǹ,out Ƿ))throw new Exception(
$"{GetType().Name} :: Couldn't find {ǹ} delegate of type {typeof(Ǽ)}");Ǹ=Ƿ as Ǽ;if(Ǹ==null)throw new Exception(
$"{GetType().Name} :: Delegate {ǹ} is not type {typeof(Ǽ)}, instead it's: {Ƿ.GetType()}");}public void Ƕ(ICollection<MyDefinitionId>Ǎ)=>ʖ?.Invoke(Ǎ);public void ǵ(ICollection<MyDefinitionId>Ǎ)=>ʕ?.Invoke(Ǎ);
public void Ǵ(ICollection<MyDefinitionId>Ǎ)=>ʔ?.Invoke(Ǎ);public bool ǻ(Sandbox.ModAPI.Ingame.IMyTerminalBlock ǜ,IDictionary<
string,int>Ǎ)=>ʓ?.Invoke(ǜ,Ǎ)??false;public MyTuple<bool,int,int>ǐ(long Ǐ)=>Ɇ?.Invoke(Ǐ)??new MyTuple<bool,int,int>();public
void ǎ(Sandbox.ModAPI.Ingame.IMyTerminalBlock ǉ,IDictionary<Sandbox.ModAPI.Ingame.MyDetectedEntityInfo,float>Ǎ)=>ǲ?.Invoke(ǉ
,Ǎ);public MyDetectedEntityInfo?ǌ(long ǋ,int ǈ=0)=>Ǔ?.Invoke(ǋ,ǈ);public bool Ǌ(Sandbox.ModAPI.Ingame.IMyTerminalBlock ǉ,
long Á,int ǈ=0)=>ǰ?.Invoke(ǉ,Á,ǈ)??false;public MyDetectedEntityInfo?Ǉ(Sandbox.ModAPI.Ingame.IMyTerminalBlock q,int ǅ=0)=>ǯ?
.Invoke(q,ǅ);public void ǆ(Sandbox.ModAPI.Ingame.IMyTerminalBlock q,long Á,int ǅ=0)=>Ǯ?.Invoke(q,Á,ǅ);public void Ǒ(
Sandbox.ModAPI.Ingame.IMyTerminalBlock q,bool ǒ=true,int ǅ=0)=>ǭ?.Invoke(q,ǒ,ǅ);public void ǚ(Sandbox.ModAPI.Ingame.
IMyTerminalBlock q,bool Ǚ,bool ǒ,int ǅ=0)=>Ǭ?.Invoke(q,Ǚ,ǒ,ǅ);public bool ǘ(Sandbox.ModAPI.Ingame.IMyTerminalBlock q,int ǅ=0,bool Ǘ=true
,bool Ǜ=false)=>ǫ?.Invoke(q,ǅ,Ǘ,Ǜ)??false;public float ǖ(Sandbox.ModAPI.Ingame.IMyTerminalBlock q,int ǅ)=>Ǫ?.Invoke(q,ǅ)
??0f;public bool Ǖ(Sandbox.ModAPI.Ingame.IMyTerminalBlock q,IList<string>Ǎ,int ǅ=0)=>ǩ?.Invoke(q,Ǎ,ǅ)??false;public void ǔ
(Sandbox.ModAPI.Ingame.IMyTerminalBlock q,IList<string>Ǎ,int ǅ=0)=>Ǩ?.Invoke(q,Ǎ,ǅ);public void ȇ(Sandbox.ModAPI.Ingame.
IMyTerminalBlock q,float ȸ)=>ǧ?.Invoke(q,ȸ);public bool ȶ(Sandbox.ModAPI.Ingame.IMyTerminalBlock q,long ȳ,int ǅ)=>Ǧ?.Invoke(q,ȳ,ǅ)??
false;public bool ȵ(Sandbox.ModAPI.Ingame.IMyTerminalBlock q,long ȳ,int ǅ)=>ǥ?.Invoke(q,ȳ,ǅ)??false;public Vector3D?ȴ(Sandbox
.ModAPI.Ingame.IMyTerminalBlock q,long ȳ,int ǅ)=>Ǥ?.Invoke(q,ȳ,ǅ)??null;public float Ȳ(Sandbox.ModAPI.Ingame.
IMyTerminalBlock q)=>ǣ?.Invoke(q)??0f;public float ȱ(Sandbox.ModAPI.Ingame.IMyTerminalBlock q)=>Ǣ?.Invoke(q)??0f;public float Ȱ(
MyDefinitionId ȯ)=>ǡ?.Invoke(ȯ)??0f;public bool Ȯ(long ȷ)=>Ǡ?.Invoke(ȷ)??false;public bool ȭ(Sandbox.ModAPI.Ingame.IMyTerminalBlock q)
=>ǟ?.Invoke(q)??false;public float Ʌ(long ȷ)=>Ǟ?.Invoke(ȷ)??0f;public string Ʉ(Sandbox.ModAPI.Ingame.IMyTerminalBlock q,
int ǅ)=>Ǳ?.Invoke(q,ǅ)??null;public void Ƀ(Sandbox.ModAPI.Ingame.IMyTerminalBlock q,int ǅ,string ɂ)=>ǝ?.Invoke(q,ǅ,ɂ);
public void Ɂ(Action<Vector3,float>ȿ)=>ǳ?.Invoke(ȿ);public void ɀ(Action<Vector3,float>ȿ)=>Ȇ?.Invoke(ȿ);public float ȼ(long ȷ)
=>Ȅ?.Invoke(ȷ)??0f;public long Ȼ(Sandbox.ModAPI.Ingame.IMyTerminalBlock q)=>ȃ?.Invoke(q)??-1;public Matrix Ⱥ(Sandbox.
ModAPI.Ingame.IMyTerminalBlock q,int ǅ)=>Ȃ?.Invoke(q,ǅ)??Matrix.Zero;public Matrix ȹ(Sandbox.ModAPI.Ingame.IMyTerminalBlock q,
int ǅ)=>ȁ?.Invoke(q,ǅ)??Matrix.Zero;}public class Ȭ{public double ȣ;public double Ţ;public double Ș;public double ȗ;public
double Ȗ;public double ȕ;public bool Ȕ;public bool ȓ;public Ȭ(double Ȓ,double ȑ,double Ȑ,double ȏ,double Ȏ,double ȍ,bool Ȍ,
bool ȋ){ȣ=Ȓ;Ţ=ȑ;Ș=Ȑ;ȗ=ȏ;Ȗ=Ȏ;ȕ=ȍ;Ȕ=Ȍ;ȓ=ȋ;}public Vector3D Ȋ(ref Vector3D ȉ,ref Vector3D Ȉ,ref Vector3D ș,ref Vector3D Ƥ,Ū Á){
Vector3D ȫ=(Ȕ?Ȉ:Ȉ-Ƥ);Vector3D ȩ=ȉ-ș;double Ȩ=(Ţ==0?0:(Ș-ȣ)/Ţ);double ȧ=(0.5*Ţ*Ȩ*Ȩ)+(ȣ*Ȩ)-(Ș*Ȩ);double Ȟ=(Ș*Ș)-ȫ.LengthSquared();
double ȝ=2*((ȧ*Ș)-ȫ.Dot(ȩ));double Ȝ=(ȧ*ȧ)-ȩ.LengthSquared();double Ȧ=ȟ(Ȟ,ȝ,Ȝ);if(double.IsNaN(Ȧ)||Ȧ<0){return new Vector3D(
double.NaN);}Vector3D ȥ;if(Á.Ţ.LengthSquared()>0.1){ȥ=ȉ+(ȫ*Ȧ)+(0.5*Á.Ţ*Ȧ*Ȧ);}else{ȥ=ȉ+(ȫ*Ȧ);}if(Ȕ&&Ș>0){int ț=(int)Math.
Ceiling(Ȧ*60);Vector3D Ȫ;Vector3D Ȥ;Vector3D Ȣ;Vector3D ȡ;Ȫ=Vector3D.Normalize(ȥ-ș);Ȥ=(Ȫ*Ţ)/60;Ȣ=ș;ȡ=Ƥ+(Ȫ*ȣ);for(int Ǝ=0;Ǝ<ț;Ǝ
++){ȡ+=Ȥ;double Ƞ=ȡ.Length();if(Ƞ>Ș){ȡ=ȡ/Ƞ*Ș;}Ȣ+=(ȡ/60);if((Ǝ+1)%60==0){if(Vector3D.Distance(ș,Ȣ)>ȗ){return ȥ;}}}return ȥ+
ȥ-Ȣ;}else{return ȥ;}}public double ȟ(double Ȟ,double ȝ,double Ȝ){if(Ȟ==0){return-(Ȝ/ȝ);}double ț=(ȝ*ȝ)-(4*Ȟ*Ȝ);if(ț<0){
return Double.NaN;}ț=Math.Sqrt(ț);double ҡ=(-ȝ+ț)/(2*Ȟ);double Ț=(-ȝ-ț)/(2*Ȟ);return(ҡ>0?(Ț>0?(ҡ<Ț?ҡ:Ț):ҡ):Ț);}