# Realdolmen-HATEOAS

## About the project

A flexible and customisable package to add HATEOAS support to your API.

HATEOAS (Hypermedia As The Engine Of Application State) is a component of the REST guidelines. Its goal is to allow a client to discover the functionality of an API by means of context sensitive hyperlinks sent by the API alongside the requested data. For instance, an API method that returns the personal information of an employee would also send a link to an index of all employees, as well as a link to confidential personal data and POST and DELETE links if the client is authorised.

This package aims to implement this feature in a way that is both powerful and simple to use.

## Getting Started

The package modifies the content that is returned by your API methods, adding relevant HATEOAS links. "Rulesets" are used to define which links should be added to the output of which methods in a reusable fashion.

### Prerequisites

What things you need to install the software and how to install them

### Installing

Fork the project via:

`git clone https:://github.com/MAximilianCoutuer/Realdolmen-HATEOAS.git`

The project will become available through NuGet shortly.

## Usage

### Rulesets

Create one or more rulesets to define which links should be added and what they should look like.

A ruleset is a class that extends the abstract class `HateoasRulesetBase`. It contains logic to indicate which links should be added to the output. It typically overrides the following fields and methods:

* `(bool)AppliesToEachListItem`: Indicates whether the ruleset should apply to each item in a list or to the list as a whole. Defaults to false if not overridden.
* `GetLinks(IIsHateoasEnabled item)`: Returns a List<HateoasLink>. To create a link, use the `HateoasLinkBuilder.Build()` method to generate a basic link based on the parameters passed in, and the methods AddTitle, AddType, AddHreflang, AddMedia and ExtendQueryString to add further data.

For an example, see the `ExampleRulesetFullLinks` class in ExampleAPI.

Any request parameters passed to the ruleset (see below) will be available via the `Parameters` field and can be used in business logic. Additionally, if the ruleset is applied to a list of objects, the `Parameters` field will automatically contain the "RD-ListId" and "RD-ListCount" keys.

### Controller

Decorate your controller method with the following attribute:

`[AddHateoasLinks(new[] { "interesting", "request", "parameters" }, new[] { typeof(NameOfRuleset), typeof(NameOfOtherRuleset) })]`

Any request parameters you specify will be passed on to the ruleset (see above). If multiple rulesets are specified, links from each ruleset will be added in order.

### Model classes

Implement IIsHateoasEnabled in your model classes, and add the following property:

`// implements IIsHateoasEnabled
[NotMapped]
[JsonProperty(PropertyName = "_links")]
List<HateoasLink> IIsHateoasEnabled.Links { get; set; } = new List<HateoasLink>();`

### Remarks

The output of your API does not need to be formatted in JSON for this package to work.

## License

This project is licensed under the MIT License.

## Authors

* **Maximilian Coutuer** - [Github](https://github.com/MaximilianCoutuer) | [LinkedIn](https://be.linkedin.com/in/maximilian-coutuer-0ba4a517)

See also the list of [contributors](https://github.com/your/project/contributors) who participated in this project.

## Acknowledgments

* Hat tip to anyone whose code was used
* Inspiration
* etc
