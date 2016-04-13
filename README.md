##Basics

There is no web API for ninjawords.com but certain requests can yield barebones HTML. Making it easy to parse into workable data. This API uses regex patterns to get information from downloaded webpages. The pages download extremely fast (around 150ms). NinjaWords can display multiple term results on one page. Meaning the API can yield results for multiple terms with one request. 

The  data break down is simple and resembles how data is displayed on the site:

Term<br/>
-Text<br />
-Entries<br />
-Synonyms<br />
-Defined
  
Entry<br />
-Definition<br />
-Example<br />
-Category<br />
>Note Entries are listed under a single category on the site, but for the sake of usability I have specified the category for each entry.
  
##Usage
Using the lib is simple, all of the methods exposed are in the static class "Ninja". Be sure to handle the potential WebException web dependant methods.

To get definitions for terms:
```C#
            var words = new[] {"I", "Write", "Code"};
            var ninjaTerms = await Ninja.GetTermsAsync(words);
            
            foreach (var term in ninjaTerms)
                Debug.WriteLine(term);
```

You may also specify terms comma separated:
```C#
            var ninjaTerms = await Ninja.GetTermsAsync("I,write,code");
            var ninjaTerm = await Ninja.GetTermsAsync("foo");
```

>You can identify words that could not be defined by checking the defined property of each NinjaTerm

To get one random definition:
```C#
var term = await Ninja.GetRandomTermAsync();
```

>Note the "Defined" property will always be true.

NinjaWords is case sensitive (to better organize terms). For instance, the word "english" is defined differently from "English". As well "Hello World" is defined whereas "hello world" is not. Character capitalization after the first letter for each word seems to be insignificant. 
