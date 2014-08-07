﻿using System.Data.Entity;

namespace TitleTurtle.Models
{
    public class HomeContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Edit> Edits { get; set; }
        public DbSet<Media> Medias { get; set; }
        public DbSet<MediaInArticle> MediaInArticles { get; set; }
        public DbSet<UserPhoto> UserPhotos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<PersonalData> PersonalDatas { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagInArticle> TagInAricles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Follower> Followers { get; set; }
        public DbSet<Like> Likes { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<HomeContext>(null);
            base.OnModelCreating(modelBuilder);
        }
    }
}

