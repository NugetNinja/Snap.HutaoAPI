// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.HutaoAPI.Models.Statistics;

public record LevelTeamUsage(FloorIndex Level, IEnumerable<Rate<Team>> Teams);

public record FloorIndex(int Floor, int Index);

public record Team(string UpHalf, string DownHalf);
