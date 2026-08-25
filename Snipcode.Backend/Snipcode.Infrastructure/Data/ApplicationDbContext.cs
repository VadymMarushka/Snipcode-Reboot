using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Snipcode.Domain.Entities;

namespace Snipcode.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Snippet> Snippets => Set<Snippet>();
    public DbSet<SnippetGroup> SnippetGroups => Set<SnippetGroup>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<SnippetTag> SnippetTags => Set<SnippetTag>();
    public DbSet<GroupTag> GroupTags => Set<GroupTag>();
    public DbSet<UserFavoriteSnippet> UserFavoriteSnippets => Set<UserFavoriteSnippet>();
    public DbSet<UserFavoriteGroup> UserFavoriteGroups => Set<UserFavoriteGroup>();

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Composite primary keys for join tables
        builder.Entity<SnippetTag>()
            .HasKey(st => new { st.SnippetId, st.TagId });

        builder.Entity<GroupTag>()
            .HasKey(gt => new { gt.GroupId, gt.TagId });

        builder.Entity<UserFavoriteSnippet>()
            .HasKey(ufs => new { ufs.UserId, ufs.SnippetId });

        builder.Entity<UserFavoriteGroup>()
            .HasKey(ufg => new { ufg.UserId, ufg.GroupId });

        // Relationships configuration
        builder.Entity<Snippet>()
            .HasOne(s => s.Author)
            .WithMany(u => u.Snippets)
            .HasForeignKey(s => s.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<SnippetGroup>()
            .HasOne(g => g.Owner)
            .WithMany(u => u.Groups)
            .HasForeignKey(g => g.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);

        // Store enums as strings in DB (for readability)
        builder.Entity<Snippet>()
            .Property(s => s.Technology)
            .HasConversion<string>();

        builder.Entity<SnippetGroup>()
            .Property(g => g.Category)
            .HasConversion<string>();
    }
}