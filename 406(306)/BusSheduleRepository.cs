class BusSheduleRepository
{
    private List<BusShedule> Shedules = [ new BusShedule() 
    { 
        Id = Guid.NewGuid(),
        BusNumber = "12",
        StopComplex = "Веревочный",
        DateTime = new TimeOnly(15,07,00)
    }];
    public void Create(BusShedule shedule)
    {
        Shedules.Add(shedule);
    }

    public List<BusShedule> Read()
    {
        return Shedules;
    }

    public void Update(Guid id, 
        string? stopComplex,
        TimeOnly? dateTime, 
        string? busNumber)
    {
        var item = Shedules.FirstOrDefault(s => s.Id == id);
        if (item == null)
            throw new Exception("Нет такого");
        if(stopComplex != null)
            item.StopComplex = stopComplex;
        if (dateTime.HasValue)
            item.DateTime = dateTime.Value;
        if(busNumber != null)
            item.BusNumber = busNumber;
    }

    public void Delete(Guid id)
    {
        var item = Shedules.FirstOrDefault(s => s.Id == id);
        if (item == null)
            throw new Exception("Нет такого");
        Shedules.Remove(item);
    }
}

//BusNumber автобус останавливается на
//остановке StopComplex в DateTime.Time
