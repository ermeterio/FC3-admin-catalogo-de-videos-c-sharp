﻿using FC.Codeflix.Catalog.Application.Exceptions;
using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Repository;
using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using Microsoft.EntityFrameworkCore;

namespace FC.Codeflix.Catalog.Infra.Data.EF.Repositories;
public class CastMemberRepository : ICastMemberRepository
{
    private readonly CodeflixCatalogDbContext _context;
    private DbSet<CastMember> _castMembers => _context.Set<CastMember>();

    public CastMemberRepository(CodeflixCatalogDbContext context) 
        => _context = context;

    public async Task Insert(CastMember aggregate, CancellationToken cancellationToken) 
        => await _castMembers.AddAsync(aggregate, cancellationToken);

    public Task Delete(CastMember aggregate, CancellationToken _)
        => Task.FromResult(_castMembers.Remove(aggregate));

    public async Task<CastMember> Get(Guid id, CancellationToken cancellationToken)
    {
        var castmember = await _castMembers.AsNoTracking().FirstOrDefaultAsync(
            x => x.Id == id,
            cancellationToken
        );
        NotFoundException.ThrowIfNull(castmember, $"CastMember '{id}' not found.");
        return castmember!;
    }

    public async Task<SearchOutput<CastMember>> Search(SearchInput input, CancellationToken cancellationToken)
    {
        var items = await _castMembers.AsNoTracking().ToListAsync();
        return new SearchOutput<CastMember>(
            input.Page,
            input.PerPage,
            items.Count,
            items.AsReadOnly()
        );
    }

    public Task Update(CastMember aggregate, CancellationToken _) 
        => Task.FromResult(_castMembers.Update(aggregate));
}
