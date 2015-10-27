var Prettifier = {
    prettifiedNumbers: [],
    category: function (abbrev) {
        switch (abbrev) {
            case "m":
                return "Million";
            case "b":
                return "Billion";
            case "t":
                return "Tillion";
            default:
                return "NA"
        }
    },
    abbrNum: function (number, decPlaces) {
        //Ignoring numbers < 1M
        if (number < 1000000) {
            return number;
        }
        var orig = number;
        var dec = decPlaces;
        // 2 decimal places => 100, 3 => 1000, etc
        decPlaces = Math.pow(10, decPlaces);

        // Enumerate number abbreviations
        var abbrev = ["k", "m", "b", "t"];
        var prettifiedCategory = "";
        // Go through the array backwards, so we do the largest first
        for (var i = abbrev.length - 1; i >= 0; i--) {

            // Convert array index to "1000", "1000000", etc
            var size = Math.pow(10, (i + 1) * 3);

            // If the number is bigger or equal do the abbreviation
            if (size <= number) {
                // Here, we multiply by decPlaces, round, and then divide by decPlaces.
                // This gives us nice rounding to a particular decimal place.
                var number = Math.round(number * decPlaces / size) / decPlaces;

                // Handle special case where we round up to the next abbreviation
                if ((number == 1000) && (i < abbrev.length - 1)) {
                    number = 1;
                    i++;
                }
                // Add the letter for the abbreviation
                number += abbrev[i];
                prettifiedCategory = Prettifier.category(abbrev[i]);
                // We are done... stop
                break;
            }
        }
        console.log('Prettify(' + orig + ', ' + dec + ') = ' + number);
        //Push Only Prettified Numbers to be inserted to the DB
        Prettifier.prettifiedNumbers.push({ orginal: orig, prettfied: number, category: prettifiedCategory });
        return number;
    },
    prettify: function (orginalText) {
        var tempStr = orginalText;
        //Extract Numbers
        var numbers = tempStr.match(/[-+]?\d+(\.\d+)?/g);
        //convert comma Separated String to Array of Token Numbers 
        //Make sure numbers is string!!!
        numbers = numbers + '';
        var numbersArray = numbers.split(',');
        //Replacing Numbers with Pretty Formats
        for (i = 0; i < numbersArray.length; i++) {
            var uglyNumber = parseFloat(numbersArray[i].toString());
            if (uglyNumber) {
                //Prettify The Numbers
                var prettyNumberStr = Prettifier.abbrNum(uglyNumber, 1);
                //Build RegEx to replace Ugly Numbers with Pretty ones    
                var regEx = new RegExp('(\\b|\\w)' + numbersArray[i].toString() + '(\\b|\\w)', 'm');
                //Replace Ugly Numbers with Pretty formatted one.
                var resStr = tempStr.replace(regEx, prettyNumberStr);
                tempStr = resStr;
            }
        }
        //Update the output textarea with the prettified text
        $("#PrettifiedTextArea").val(tempStr);

        //Update the Database thru Ajax Call
        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();
        for (i = 0; i < Prettifier.prettifiedNumbers.length; i++) {
            $.ajax({
                type: "POST",
                url: "/PrettifiedNumbers/Create",
                data: { __RequestVerificationToken: token, PrettifiedNumber: Prettifier.prettifiedNumbers[i].prettfied, OrginalNumber: Prettifier.prettifiedNumbers[i].orginal, UserId: CurrentUserID, PrettifiedCategory: Prettifier.prettifiedNumbers[i].category },
                cache: false,
                async: true,
                success: function (result) {
                    console.log("Ajax Call Returned Successfully.");
                }
            });
        }
        //Empty PrettifieNumbers Array
        Prettifier.prettifiedNumbers.length = 0;
    } //End of prettify
};