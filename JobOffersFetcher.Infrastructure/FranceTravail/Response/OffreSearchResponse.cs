
namespace JobOffersFetcher.Infrastructure.FranceTravail.Response;

public class OffreSearchResponse
{
    public Resultats[] resultats { get; set; }
    public FiltresPossibles[] filtresPossibles { get; set; }
}

public class Resultats
{
    public string id { get; set; }
    public string intitule { get; set; }
    public string description { get; set; }
    public DateTime dateCreation { get; set; }
    public DateTime dateActualisation { get; set; }
    public LieuTravail lieuTravail { get; set; }
    public string romeCode { get; set; }
    public string romeLibelle { get; set; }
    public string appellationlibelle { get; set; }
    public Entreprise entreprise { get; set; }
    public string typeContrat { get; set; }
    public string typeContratLibelle { get; set; }
    public string natureContrat { get; set; }
    public string experienceExige { get; set; }
    public string experienceLibelle { get; set; }
    public Formations[] formations { get; set; }
    public Permis[] permis { get; set; }
    public Competences[] competences { get; set; }
    public Salaire salaire { get; set; }
    public string dureeTravailLibelle { get; set; }
    public string dureeTravailLibelleConverti { get; set; }
    public bool alternance { get; set; }
    public Contact contact { get; set; }
    public Agence agence { get; set; }
    public int nombrePostes { get; set; }
    public bool accessibleTH { get; set; }
    public string deplacementCode { get; set; }
    public string deplacementLibelle { get; set; }
    public string qualificationCode { get; set; }
    public string qualificationLibelle { get; set; }
    public string codeNAF { get; set; }
    public string secteurActivite { get; set; }
    public string secteurActiviteLibelle { get; set; }
    public QualitesProfessionnelles[] qualitesProfessionnelles { get; set; }
    public OrigineOffre origineOffre { get; set; }
    public bool offresManqueCandidats { get; set; }
    public string experienceCommentaire { get; set; }
    public Langues[] langues { get; set; }
    public string complementExercice { get; set; }
}

public class LieuTravail
{
    public string libelle { get; set; }
    public double latitude { get; set; }
    public double longitude { get; set; }
    public string codePostal { get; set; }
    public string commune { get; set; }
}

public class Entreprise
{
    public string nom { get; set; }
    public string description { get; set; }
    public string logo { get; set; }
    public string url { get; set; }
    public bool entrepriseAdaptee { get; set; }
}

public class Formations
{
    public string codeFormation { get; set; }
    public string domaineLibelle { get; set; }
    public string niveauLibelle { get; set; }
    public string exigence { get; set; }
    public string commentaire { get; set; }
}

public class Permis
{
    public string libelle { get; set; }
    public string exigence { get; set; }
}

public class Competences
{
    public string code { get; set; }
    public string libelle { get; set; }
    public string exigence { get; set; }
}

public class Salaire
{
    public string libelle { get; set; }
    public string complement1 { get; set; }
    public string complement2 { get; set; }
    public string commentaire { get; set; }
}

public class Contact
{
    public string nom { get; set; }
    public string coordonnees1 { get; set; }
    public string courriel { get; set; }
    public string coordonnees2 { get; set; }
    public string coordonnees3 { get; set; }
    public string urlPostulation { get; set; }
}

public class Agence
{
    public string courriel { get; set; }
}

public class QualitesProfessionnelles
{
    public string libelle { get; set; }
    public string description { get; set; }
}

public class OrigineOffre
{
    public string origine { get; set; }
    public string urlOrigine { get; set; }
}

public class Langues
{
    public string libelle { get; set; }
    public string exigence { get; set; }
}

public class FiltresPossibles
{
    public string filtre { get; set; }
    public Agregation[] agregation { get; set; }
}

public class Agregation
{
    public string valeurPossible { get; set; }
    public int nbResultats { get; set; }
}

