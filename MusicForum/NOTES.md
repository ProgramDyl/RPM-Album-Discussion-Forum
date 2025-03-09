# Notes

## Project Story

### Database Issues

- I was having serious difficulty adding the `ApplicationUser` to the discussion model. 
The issue was because the first migration that was added had the ApplicationUser added 
also. This creates a circular issue because there are contradicting relationships in the
database. 

- I removed the `ApplicationUser` from the `Discussion` model and then ran the migratiion
again. The migration was still not successful. The error was due to a conflit with the FK 
constraint when trying to add to **BOTH** the Discussion and the Comment models. A little 
bit of panic, a litany of google searches, and about 4 hours later, I managed to solve it. 
Eventually I had to delete the entire database and start from scratch. No big deal because 
I can tag the original db as -old and then create a new one. This means having to go through 
the whole project correcting `MusicForumContext` to `RPMForumContext`. 

The error was a cascading delete issue. If the parent entity is deleted, the foreign key values
of the child entities do not match the primary key of _any_ parent entity. This is is considered
an invalid state. 

- Digging deeply into the database was a good learning experience. I was able to see how the
SQL server is structured and how the migrations are created. I got to look at migration files
with a closer eye and see how the relationships are created. This was helpful. 


- I also learned that specifying `DeleteBehavior.Cascade` for both discussion and user would 
result in a deletion of both entities, which makes the DB schema more liable for cascading 
deletes. If a thread got to 1000 comments and the user deleted their account, it would start 
burning through the whole database. Pretty wild stuff. Enttty Framework 

```
COMMANDS USED: 

remove-migration
add-migration
update-database
drop-database
```



_"You gotta know when to hold 'em."_<br/>
-Kenny Rogers

