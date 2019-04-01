# Simple Slot Machine

## Technology stack

 - `ASP.Net` single razor page for website
 - `ASP.Net Core` api
 - `.Net Core` library contains the `SlotsService` which handles all the logic of the slot machine.

Both the web and api project should be running for the app to function correctly.

## Some considerations for future development

 - Install an IoC container rather than manually creating concrete implementations (e.g. `SlotsController` constructor).
 - Setup TypeScript rather than use JavaScript's revealing module pattern.
 - Replace display text on the slot machine with actual images. Winning rows could also be highlighted by including the coefficient of each row. In general the front end could be made a lot more lively.
 - API and browser testing of the app.
 - Replace API with Azure functions so the service is more lightweight
 - Replace website with React (or similar) SPA.