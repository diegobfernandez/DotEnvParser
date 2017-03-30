# DotEnvParser

## Installing

The easiest way to get started is by installing the available NuGet packages

## Getting Started

After installing create a file named `.env` in your project directory (the same of your *.cs|fs|vbproj) and add any variable you would like to.

```
APP_ENV=development
DB_CONNECTION_STRING=You can add anything after the = sign
```

It's important to note that it has strict rules.

- The name must contains only letters (lower or uppercase), numbers and _ (underscores)
- The name must begin with a letter
- The name and value must be separated by the equal sign (=) and no space before it
- Each variable must be on a line
