using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using TabloidMVC.Models;


namespace TabloidMVC.Repositories
{
    public class TagRepository : BaseRepository, ITagRepository
    {

        public TagRepository(IConfiguration config) : base(config) { }
       
    //Get all of the tags
        public List<Tag> GetAllTags()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT id, name FROM Tag ORDER BY name";
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
    //Add a new tag
        public void AddTag(Tag tag)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Tag (Name)
                        VALUES(@Name);";
                    cmd.Parameters.AddWithValue("@Name", tag.Name);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    //Get tag by the id
        public Tag GetTagById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, [Name]
                        FROM Tag
                        WHERE Id = @id
                        ";
                   cmd.Parameters.AddWithValue("@id", id);
                  var reader = cmd.ExecuteReader();

                    if(reader.Read())
                    {
                        Tag tag = new Tag
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };
                        reader.Close();
                        return tag;
                    }
                    else
                    {
                        reader.Close();
                        return null;
                    }
                }
            }
        }


    //Delete a tag
        public void RemoveTag(int tagId)
        { 
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        DELETE FROM Tag
                        WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", tagId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

    //Update a tag
        public void UpdateTag(Tag tag)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE Tag
                        SET 
                            Name = @name,
                        WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@name", tag.Name);
                    cmd.Parameters.AddWithValue("@id", tag.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
