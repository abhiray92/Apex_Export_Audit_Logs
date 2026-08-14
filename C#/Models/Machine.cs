using System.Collections.Generic;

public class Machine
{
    public string SystemId { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
}

public class MachineConfig
{
    public List<Machine> Machines { get; set; } = new List<Machine>();
}
