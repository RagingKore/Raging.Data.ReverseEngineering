Raging.Data.ReverseEngineering
==============================

Creates an entity framework 6 code first database model from a SQL Server database.

I just found that existing tools where not as robust, configurable and extensible as I wanted them to be. 
And when some of them can't even generate classes for some legacy databases I've been using or provide features that favour anti patterns, I thought I should give it a try and do my own.

The ultimate goal would be to generate a perfect code first model that would allow us not only to completely re-create the target database but also allow us to change the underlying database technology from, as an example, SQL to Postgres.

Right now it just works, but needs a lot work and more tests. It's like a pre alpha 0.1 lol.

I'm still trying to refine some ideas and major changes will probably happen.

Features:

* Pluralization of generated identifiers for properties and collections, by providing an IPluralizationService interface, much like EF 6 Contrib. Default is English.
* Camel case, which will also remove invalid characters from generated identifiers.
* Default values are automatically generated in the constructor no matter if the columns is required or not.
* Table filtering provided by a white list and a black list. (May allow regex expression in the future.)
* Global name overrides by convention. Example: From="dbo.tbl_Person.Person_Name" To="Name" to rename column or From="dbo.tbl_Person" To="Person"
* Many to Many relations are properly named. It seems nothing in the world does this... don't really know why.
* Computed columns are generated as read only. (No setters.)
* Supports Spatial.DbGeometry and Spatial.DbGeography.

Roadmap:

* Consolidate architecture ideas...
* Allow better configuration either by creating a special section in .config or in the t4 itself.
* Generation of multiple files.
* Create Nuget package.
* Default values created in constructor initialization need to be more robust. Some edge cases must be implemented regarding invalid values.
* Allow Views generation.
* Allow Enum generation by convention or configuration.
* Allow SQL descriptions to be generated as description attributes or comments.
* Allow multiple Schemas, represented by diferent namespaces.
* Configurable generation of implement.
* Eventually allow Stored Procedure generation...
* Support for Postgres, Sql Server Compact, MySQL and others.

