/* 
Based on Adam Tibi source code
*/

function MultipleFieldsValidatorEvaluateIsValid(val) {
    var validatorValue = '';
    var initialvalues = null;
    var allfieldscanbeblank = Boolean(val.allcanbeblank == 'true');
    
    controltovalidateIDs = val.controlstovalidate.split(',');
    if (val.initialValues != null && val.initialValues != '') {
        initialvalues = val.initialValues.split(',');
    }
    
    switch (val.condition) {
        case 'OR':
            for (var controltovalidateIDIndex in controltovalidateIDs) {
                var controlID = controltovalidateIDs[controltovalidateIDIndex];
                validatorValue = ValidatorTrim(ValidatorGetValue(controlID));
                if (validatorValue != '') {
                    if (initialvalues != null) {
                        for (var initialValueIndex in initialvalues) {
                            if (validatorValue != initialvalues[initialValueIndex])
                                return true;
                        }
                    }
                    else {
                        return true;
                    }
                }
            }
          
            return allfieldscanbeblank;
            break;
        case 'XOR':
            for (var controltovalidateIDIndex in controltovalidateIDs) {
                var controlID = controltovalidateIDs[controltovalidateIDIndex];
                if (controltovalidateIDIndex == '0') {
                    var previousResult = !(ValidatorTrim(ValidatorGetValue(controlID)) == '');
                    continue;
                }
                var currentResult = !(ValidatorTrim(ValidatorGetValue(controlID)) == '');
                if (currentResult != previousResult) {
                    return true;
                }
                previousResult != currentResult;
            }
            return false;
            break;
        case 'AND':
            var foundValue = false;
            var foundNoValue = false;

            for (var controltovalidateIDIndex in controltovalidateIDs) {
                var controlID = controltovalidateIDs[controltovalidateIDIndex];
                validatorValue = ValidatorTrim(ValidatorGetValue(controlID));

                if (validatorValue == '') {
                    if (allfieldscanbeblank == false) return false;
                    else foundNoValue = true;
                }
                else if (initialvalues != null) {
                    for (var initialValueIndex in initialvalues) {
                        if (validatorValue == initialvalues[initialValueIndex]) {
                            if (allfieldscanbeblank == false) return false;
                            else foundNoValue = true;
                        }
                    }
                }
                else {
                    foundValue = true;
                }
            }
            if (allfieldscanbeblank && foundValue && !foundNoValue) return true;
            else if (allfieldscanbeblank && foundNoValue && !foundValue) return true;
            else if (allfieldscanbeblank == false) return true;
            else return false;
            break;
    }
    return false;
}
