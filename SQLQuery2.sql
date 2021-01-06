SELECT c.Id, c.PostId, c.UserProfileId, c.Subject, c.Content, c.CreateDateTime,
                               p.Id, p.Title, p.Content, p.ImageLocation, p.CreateDateTime, p.PublishDateTime,
                               p.IsApproved, p.CategoryId, p.UserProfileId,
                               u.Id, u.DisplayName, u.FirstName, u.LastName, u.Email, 
                               u.CreateDateTime, u.ImageLocation, u.UserTypeId,
                               ut.Id, ut.Name
                        FROM Comment c
                        LEFT JOIN Post p  ON p.Id = c.PostId
                        LEFT JOIN UserProfile u ON u.Id = p.UserProfileId
                        LEFT JOIN UserType ut ON ut.Id = u.UserTypeId
                        WHERE c.PostId = 2