namespace JobOffersFetcher.Core.Entities;

public class Entreprise
{
    public string Nom { get; set; }
    public string Description { get; set; }
    public string Logo { get; set; }
    public string Url { get; set; }
    public bool EntrepriseAdaptee { get; set; }
    
    public override bool Equals(object? obj)
    {
       
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Entreprise other = (Entreprise)obj;
        return Nom == other.Nom;
    }

    public override int GetHashCode()
    {
        return Nom.GetHashCode();
    }
}