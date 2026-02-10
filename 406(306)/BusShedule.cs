class BusShedule
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string StopComplex { get; set; }
    public TimeOnly DateTime { get; set; }
    public string BusNumber { get; set; }
}

//BusNumber автобус останавливается на
//остановке StopComplex в DateTime.Time
