# RD-HATEOAS

## About the project

A flexible and customisable package to add HATEOAS support to your .NET Core API.

HATEOAS (Hypermedia As The Engine Of Application State) is a component of the REST guidelines. Its goal is to allow a client to discover the functionality of an API by means of context sensitive hyperlinks sent by the API alongside the requested data. For instance, an API method that returns the personal information of an employee would also send a link to an index of all employees, as well as a link to confidential personal data and POST and DELETE links if the client is authorised.

This package aims to implement this feature in a way that is both powerful and simple to use.

See `ExampleAPI` for an example of how to use the package.

## Getting Started

The package enriches the content that is returned by your API methods, adding HATEOAS links under a `_links` key in accordance with HATEOAS standards. "Rulesets" are used to define which links should be created and which objects they should be added to.

This repository also contains an `ExampleAPI` project featuring a basic API with an in-memory database to demonstrate the functionality of the package.

### Prerequisites

* .NET Core 2.2 or higher.

### Installation

Fork the project via:

`git clone https:://github.com/MaximilianCoutuer/Realdolmen-HATEOAS.git`

The project is available on NuGet as `RD-HATEOAS`. Note that it is a work in progress until 1.0.0.

## Usage

### Rulesets

To start, create one or more rulesets. A *ruleset* defines a block of links that will be added under the `_links` key somewhere in the output of your API method.

For instance, if your API method returns a list of twenty `Person` objects, each with a nationality in the form of a `Country` object, you could create a ruleset that defines three links: a link to obtain the details of a country, a link to edit the country and a link to delete the country. (In the next step, we will attach it to the country objects in the output.) Of course, you will be able to reuse this ruleset any time you return `Country` objects in some form.

A ruleset extends the abstract class `HateoasRulesetBase`. It typically overrides the following fields and methods:

* `(bool)AppliesToEachListItem`: Indicates whether the ruleset should apply to each individual item in a list or to the list as a whole, if applicable. Defaults to true.
* `GetLinks(JToken item)`: Returns a `List<HateoasLink>`, which is a list of zero or more links. This is where you build any number of links and return them. For instance, you could define a self link, a details link, and an optional edit link contingent on the user being an administrator.

Of course, it is not necessary to build links by hand. To create a link, use the `HateoasLinkBuilder.Build()` method to generate a basic link based on the parameters passed in, and the methods `AddTitle, AddType, AddHreflang, AddMedia` and `ExtendQueryString` to add further data. For self links, use `HateoasLinkBuilder.BuildSelfLink()` which will automatically generate a link to self.

Returning static links would not be very interesting. To help you build links that vary depending on the input or output of your API, the object your links will be applied to is passed to the `GetLinks` method, and any request parameters passed to the ruleset (see below) are available via the `Parameters` dictionary. Additionally, if the object is a member of a list of similar objects, the `RD-ListId` and `RD-ListCount` keys are also available via the `Parameters` dictionary.

For an example, see the `ExampleRuleset...` classes in `ExampleAPI`.

_Notice:_ If `AppliesToEachListItem` is false, the ruleset will apply to an entire list instead of the items in it. This is not common practice, but can be useful (notably when the API returns a list of objects and you wish to add a self link to this list). To accomplish this, the package will move the list into a new object under the `values` key and add the `_links` key underneath it.

### Controller

Decorate your controller method with the following attribute:

```
[AddHateoasLinks(typeof(YourPropertySet))]
```

or

```
[AddHateoasLinks(new[] { typeof(YourPropertySet), typeof(AnotherPropertySet, ... } )
```

You will need a *property set*. A property set is an options object that defines *which* ruleset should be applied to *which* object(s) in the object hierarchy of your API output.

For instance, say your API returns a list of `Person` objects, each with a `Capital` key that is a `Country` object. You created a ruleset that defines a number of country related links and now wish to apply it to the countries in your output. The property set tells the package where the countries in your output are and to apply the correct ruleset to them.

A property set implements `IHateoasPropertySet` and includes three properties:

* `Type Ruleset` The type of your ruleset.
* `List<string> Path` A list of key names the package should follow in your output to arrive at the object you want to provide with links. For instance, `{ "Country", "Capital" }` will apply links to the `Capital` object in the `Country` object in your root object or list. If this is `null`, the ruleset will be applied to the root object or list.
* `List<string> Parameters` Any request parameters you specify will be passed on from the API request to the ruleset. If there are none, use `null`.

If you have multiple property sets, it is strongly recommended to add any property sets containing rulesets that have `AppliesToEachListItem` set to `false` (for instance, the ubiquitous self link) at the bottom of the list. As mentioned above, adding links to a list will move the list behind a `values` key, which changes the structure of your output, which means any subsequent property set with a path pointing to an object inside the list will become invalid unless you modify the path accordingly.

### Remarks

Because the package needs to add properties to the object hierarchy and C# is strongly typed, it will convert each object into a JToken. Filters that run after this package will therefore lose access to the original object type.

## To do

This is a prerelease version. The following features are planned:

* Full test coverage. The test projects are currently incomplete as a result of rapid iteration on the main package.
* Verify correct functionality with XML output or other output formats.
* Add more example rulesets.

## License

This project is licensed under the MIT License. Feel free to fork and improve on it!

## Authors

* **Maximilian Coutuer** - [Github](https://github.com/MaximilianCoutuer) | [LinkedIn](https://be.linkedin.com/in/maximilian-coutuer-0ba4a517) | [Patreon](https://patreon.com/enaisiaion)

See also the list of [contributors](https://github.com/your/project/contributors) who participated in this project.

## Acknowledgments

* N.Zawada and K.Dockx, for technical and moral support
* Realdolmen, for the internship and the awesome opportunity
* Hogeschool VIVES Kortrijk, my alma mater.
