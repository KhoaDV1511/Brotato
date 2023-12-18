using UnityEngine;

public class StartGameSignals : ASignal{}
public class WaveTimeSignals : ASignal<TimePerWave>{}
public class EndWaveSignals : ASignal<int>{}
public class UpgradeWeaponSignals : ASignal<EquipmentItemInfo>{}
public class UpgradeItemSignals : ASignal<EquipmentItemInfo>{}
public class StartNewWaveSignals : ASignal{}
public class BuyItemRollClick : ASignal<int>{}
public class MergeWeaponSignals : ASignal<ElementWeaponUpgrade>{}
public class PotatoRevivalSignals : ASignal{}
public class EnemyDeathSignals : ASignal<Vector2>{}
public class PotatoDeathSignals : ASignal{}
public class UpdateDropItemPickedSignals : ASignal{}
public class PotatoPickDropItemToStoreSignals : ASignal<Vector2>{}