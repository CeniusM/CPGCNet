# CommandPromptGraphingCalculator (CPGCNet)

A command prompt based graphing calculator that allows you to define and call functions.

## Features

* Define and call functions using the syntax `Name(x) = Function body`
* Clear all defined functions and prompt using the `CLEAR` command
* Display help message using the `HELP` command

## To-do

* [ ] Implement parentheses "()".
* [ ] Use "=" to determine if it is a function declaration.
* [ ] Implement debugging to show where errors occur.
* [ ] Use polymorphism for nodes to simplify.
* [ ] Allow for multi-argument functions.
* [ ] Add support for operators "!" and "^".
* [ ] Improve expression handling for constants and variables.
* [ ] Add debugging utility.
* [ ] Change node function to use another class instead of ValueContainer.

## Example

```CPGC
f(x) = x+2
g(x) = x+5

f(2)
>> 4
g(5)
>> 10
```
