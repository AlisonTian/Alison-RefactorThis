# refactor-this
The attached project is a poorly written API in C#.

Please evaluate this project, and refactor as much of it as possible to make it production ready. We recognise your time is precious, so you do not have to finish everything, but please make a solid attempt.
When you make changes, please enter a description of these changes into the "RefactorThis - CandidateNotes" text file, as a changelog for the patch.

If there are parts that need to change but you don't have time to do so, or don't quite know how to, or you think should be handled elsewhere, 
please note these down in the "Here's what I would like to talk about at the interview:" section of the "RefactorThis - CandidateNotes" text file.

Please consider all aspects of good software engineering, and show us how you would leave your mark on this project.

## Restrictions
* The project must remain in C# .NET Core.
* The project must continue to use a Kestrel webserver.
* You can only add new libraries/packages through Nuget.
* The database must remain a SQLite database with its current setup. You are allowed to modify the schema. You are not being assessed on your knowledge of, or skills with, SQLite.

## Tips
* Just because an algorithm, method or pattern is used in this project, doesn't make it correct. Some things should be removed or changed. Tell us what they are.
* You may need to rewrite sections of this project to do an excellent job on this task. Feel free to be as bold as you like when working on this task.
* Make sure to follow C# conventions. If your personal coding style of C# differs from the standard Allman style, feel free to use it as long as you are consistent and the code is readable.
* Consider adding a test project to the solution.

## Assessment Criteria
Your submission will be assessed on the following areas:

* Code Style and Maintainability
* RESTful Architecture
* Application Correctness
* Ability to spot problems and bugs
* Exception and Error Handling
* General Database Operations
* Security / Auth

## Getting started for applicants

There should be these endpoints:

1. `GET /products?name={name}` - finds all products matching the specified name.
2. `GET /products/{id}` - gets the project that matches the specified ID - ID is a GUID.
3. `POST /products` - creates a new product.
4. `PUT /products/{id}` - updates a product.
5. `DELETE /products/{id}` - deletes a product and its options.
6. `GET /products/{id}/options` - finds all options for a specified product.
7. `POST /products/{id}/options` - adds a new product option to the specified product.
8. `DELETE /products/{id}/options/{optionId}` - deletes the specified product option.

All models are specified in the `/Models` folder, but should conform to:

**Product:**
```
{
  "Id": "01234567-89ab-cdef-0123-456789abcdef",
  "Name": "Product name",
  "Description": "Product description",
  "Price": 123.45,
  "DeliveryPrice": 12.34
}
```

**Products:**
```
{
  "Items": [
    {
      // product
    },
    {
      // product
    }
  ]
}
```

**Product Option:**
```
{
  "Id": "01234567-89ab-cdef-0123-456789abcdef",
  "ProductId": "abcd4567-89ab-cdef-0123-456789abcdef",
  "Name": "Product name",
  "Description": "Product description"
}
```

**Product Options:**
```
{
  "Items": [
    {
      // product option
    },
    {
      // product option
    }
  ]
}
```
