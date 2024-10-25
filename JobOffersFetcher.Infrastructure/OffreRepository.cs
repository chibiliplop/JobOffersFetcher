using JobOffersFetcher.Core.Entities;
using JobOffersFetcher.Core.Repository;
using JobOffersFetcher.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JobOffersFetcher.Infrastructure;

public class OffreRepository : IOffreRepository
{
    private readonly JobOffersFetcherContext _context;

    public OffreRepository(JobOffersFetcherContext context)
    {
        _context = context;
    }

    public async Task<HashSet<Entreprise>> GetAllEntreprise()
    {
        return new HashSet<Entreprise>(await _context.Set<Entreprise>().ToListAsync());
    }

    public Task<Entreprise> GetEntrepriseByName(string name)
    {
        return _context.Set<Entreprise>().Where(e => e.Nom == name).FirstOrDefaultAsync();
    }

    public Task<List<Offre>> SearchOffre(string searchTerm)
    {
        return _context.Set<Offre>()
            .Where(o => EF.Functions.Like(o.Intitule, $"%{searchTerm}%"))
            .AsNoTracking()
            .ToListAsync();
    }


    public async Task<HashSet<string>> GetAllOffreIds()
    {
        return new HashSet<string>(await _context.Set<Offre>().Select(x => x.Id).ToListAsync());
    }

    public Task AddOffre(Offre offre)
    {
        _context.Add(offre);
        return _context.SaveChangesAsync();
    }

    public Task DeleteOffre(string offreId)
    {
        return _context.Set<Offre>().Where(o => o.Id == offreId).ExecuteDeleteAsync();
    }

    public Task<Offre> GetOffreById(string offreId)
    {
        return _context.Set<Offre>().Where(o => o.Id == offreId)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public Task AddOffres(List<Offre> offres)
    {
        _context.AddRange(offres);
        return _context.SaveChangesAsync();
    }

    public Task<List<Statistic>> GetStatPerTypeContrat()
    {
        return _context.Offres
            .GroupBy(o => o.TypeContrat)
            .Select(g => new Statistic
            {
                Key = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(g => g.Count)
            .AsNoTracking()
            .ToListAsync();
    }

    public Task<List<Statistic>> GetStatPerEntreprise()
    {
        return _context.Offres
            .GroupBy(o => o.Entreprise.Nom)
            .Select(g => new Statistic
            {
                Key = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(g => g.Count)
            .AsNoTracking()
            .ToListAsync();
    }

    public Task<List<Statistic>> GetStatPerCommune()
    {
        return _context.Offres
            .GroupBy(o => o.LieuTravail.Commune)
            .Select(g => new Statistic
            {
                Key = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(g => g.Count)
            .AsNoTracking()
            .ToListAsync();
    }
}