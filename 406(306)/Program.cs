var builder = WebApplication.CreateBuilder();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
var app = builder.Build();
app.UseCors(x => x
    .AllowAnyHeader()
    .AllowAnyOrigin()
    .AllowAnyMethod());
app.UseSwagger();
app.UseSwaggerUI();

BusSheduleRepository repo = new();

var shedulePath = "/shedule";
app.MapPost(shedulePath, (BusShedule shedule)
    => repo.Create(shedule));
app.MapGet(shedulePath, () 
    => repo.Read());
app.MapPut(shedulePath + "/{id}", (Guid id, BusSheduleUpdateDTO dto)
    => repo.Update(id, dto.StopComplex, dto.DateTime, dto.BusNumber));
app.MapDelete(shedulePath + "/{id}", (Guid id) 
    => repo.Delete(id));

app.Run();

record BusSheduleUpdateDTO(
    string? StopComplex,
    TimeOnly? DateTime,
    string? BusNumber);


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
class BusShedule
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string StopComplex { get; set; }
    public TimeOnly DateTime { get; set; }
    public string BusNumber { get; set; }
}

//BusNumber автобус останавливается на
//остановке StopComplex в DateTime.Time
