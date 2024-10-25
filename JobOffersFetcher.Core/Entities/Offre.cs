namespace JobOffersFetcher.Core.Entities;

public class Offre
{
    public string Id { get; set; }
    public string Intitule { get; set; }
    public string Description { get; set; }
    public DateTime DateCreation { get; set; }
    public DateTime DateActualisation { get; set; }
    public string TypeContrat { get; set; }
    public Entreprise Entreprise { get; set; }
    public LieuTravail LieuTravail { get; set; }
    public string UrlPostulation { get; set; }
    public string Provider { get; set; }
    
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Offre other = (Offre)obj;
        return Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
    
    
}