
using System.Collections.Generic;

public class StartGameSignals : ASignal{}
public class WaveTimeSignals : ASignal<TimePerWave>{}
public class EndWaveSignals : ASignal<int>{}
public class UpgradeWeaponSignals : ASignal<EquipmentItemInfo>{}
public class UpgradeItemSignals : ASignal<EquipmentItemInfo>{}

public class StartNewWaveSignals : ASignal<List<ElementWeaponUpgrade>>{}