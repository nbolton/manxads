using System;

[Flags]
public enum RobotFlag
{
    None = 0,
    Index = 2,
    NoIndex = 4,
    Follow = 8,
    NoFollow = 16
}
