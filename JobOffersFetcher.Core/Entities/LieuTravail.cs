namespace JobOffersFetcher.Core.Entities;

public class LieuTravail
{
    public int Id { get; set; }
    public string Libelle { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string CodePostal { get; set; }
    public string Commune { get; set; }


    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        LieuTravail other = (LieuTravail)obj;
        return Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}