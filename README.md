# RD-HATEOAS

## About the project

A flexible and customisable package to add HATEOAS support to your .NET Core API.

HATEOAS (Hypermedia As The Engine Of Application State) is a component of the REST guidelines. Its goal is to allow a client to discover the functionality of an API by means of context sensitive hyperlinks sent by the API alongside the requested data. For instance, an API method that returns the personal information of an employee would also send a link to an index of all employees, as well as a link to confidential personal data and POST and DELETE links if the client is authorised.

This package aims to implement this feature in a way that is both powerful and simple to use.

## Getting Started

The package enriches the content that is returned by your API methods, adding relevant HATEOAS links. "Rulesets" are used to define which links should be added to the output of your in a reusable fashion.

### Prerequisites

* .NET Core 2.2 or higher.

### Installation

Fork the project via:

`git clone https:://github.com/MaximilianCoutuer/Realdolmen-HATEOAS.git`

The project will become available through NuGet at version 1.0.

## Usage

### Rulesets

Create one or more rulesets to define which links should be added and what they should look like.

A *ruleset* is a class that extends the abstract class `HateoasRulesetBase`. It contains the necessary logic to generate one or more links. For instance, it could define a self link, a details link, and an optional edit link contingent on the user being an administrator.

A ruleset typically overrides the following fields and methods:

* `(bool)AppliesToEachListItem`: Indicates whether the ruleset should apply to each item in a list or to the list as a whole. Defaults to true if not overridden.
* `GetLinks(JToken item)`: Returns a `List<HateoasLink>`. To create an instance of a link, use the `HateoasLinkBuilder.Build()` method to generate a basic link based on the parameters passed in, and the methods `AddTitle, AddType, AddHreflang, AddMedia` and `ExtendQueryString` to add further data.

For an example, see the `ExampleRuleset...` classes in ExampleAPI.

To help implement business logic in a ruleset, any request parameters passed to the ruleset (see below) will be available via the `Parameters` field. Additionally, if the ruleset is applied to a list or array, the `Parameters` field will automatically contain the `RD-ListId` and `RD-ListCount` keys for your convenience.

### Controller

Decorate your controller method with the following attribute:

```
[AddHateoasLinks("NameOfPropertySet")]
```

A _property set_ is an object that implements `IHateoasPropertySet` and includes three properties:

* `List<string> Parameters` Any request parameters you specify will be passed on to all ruleset(s) (see above). If none, use `null` as the first parameter.
* `List<string> Path` A list of key names the package should follow in your output to arrive at the object you want to provide with links. For instance, `{ "Country", "Capital" }` will apply links to the Capital object in the Country object in your output. This is nullable, in which case links will be added to the root object or list.
* `Type Ruleset` The type of your ruleset.

The purpose of a property set is to reduce configuration hassle.

Multiple property sets can be applied to the same method.

A *path* is a pipe separated list of property names that help the application drill down to your desired object. For instance, if your API generates a `List<Person>`, each with a `Country` property with a `Capital` property that is an object of class `City`, and you wish to add links to all capitals, your path should be `Country|Capital`. If you want to add links to the root object, use a path of `null`.

### Remarks

Applying links to a list or array will move it into a key/value pair, with the key being `values`. This is a necessary evil because adding properties directly to a list or array is not possible.

Because the package needs to add properties to the object hierarchy and C# is strongly typed, it will convert each object into a JToken. Filters that run after this package will therefore lose access to the original object type.

## To do

This is a prerelease version. The following features are planned:

* Bulletproof/idiot proof/thoroughly test the package. The package is newborn and there is no full test coverage or error handling yet, making it fairly easy to crash. This is the main priority for 1.0.
* Verify correct functionality with XML output or other output formats.
* Add more example rulesets.
* Simplify the controller decoration syntax.

## License

This project is licensed under the MIT License.

## Authors

* **Maximilian Coutuer** - [Github](https://github.com/MaximilianCoutuer) | [LinkedIn](https://be.linkedin.com/in/maximilian-coutuer-0ba4a517) | [Patreon](https://patreon.com/enaisiaion)

See also the list of [contributors](https://github.com/your/project/contributors) who participated in this project.

## Acknowledgments

* N.Zawada and K.Dockx, for technical and moral support
* Realdolmen, for the internship and the awesome opportunity
* Hogeschool VIVES Kortrijk, my alma mater.
