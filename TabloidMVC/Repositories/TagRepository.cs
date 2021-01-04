using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using TabloidMVC.Models;


namespace TabloidMVC.Repositories
{
    public class TagRepository : BaseRepository, ITagRepository
    {

        public TagRepository(IConfiguration config) : base(config) { }
        public List<Tag> GetAllTags()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT id, name FROM Tag";
                    var tags = new List<Tag>();
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Tag tag = new Tag()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                        };
                        tags.Add(tag);
                    }

                    reader.Close();
                        return tags;

                }
            }
        }

    }
}
