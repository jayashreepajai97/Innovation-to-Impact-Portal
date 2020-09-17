$(document).ready(function () {
    // resive Swagger body
    ResizeSwaggerBody();
    HideOptionsMethods();

    var AuthorizationName = "Authorization";
    var objs = document.getElementsByName(AuthorizationName);
    
    // for each "AuthorizationName" input object, we should replace it with 
    // a text area object. The new created textarea object will have a different name than
    // AuthorizationName not to be selected itseld
    for (var i = 0; i < objs.length; i++) {
        var parent = objs[i].parentElement;
        var textBox = document.createElement("textarea");
        textBox.id = objs[i].id;
        textBox.classList = "body-textarea required";
        textBox.placeholder = "(required)";
        textBox.name = "DUMMY" + AuthorizationName;
        textBox.onfocus = "MyFunction()";
        parent.appendChild(textBox);
        textBox.addEventListener("blur", function () {
            WriteNewAuthorizationStyle(this);
        }, false);

        textBox.addEventListener("dblclick", function () {
            DoubleClickEvent(this);
        }, false);
    }

    // delete all old input AuthorizationName fields
    var exists = true;
    while (exists)
    {
        var objs = document.getElementsByName(AuthorizationName);
        if( objs.length == 0 )
        {
            exists = false;
        }
        else
        {
            var parent = objs[0].parentElement;
            parent.removeChild(objs[0]);
        }
    }

    // correctly rename to AuthorizationName the new textarea fields
    exists = true;
    while (exists) {
        var objs = document.getElementsByName("DUMMY" + AuthorizationName);
        if (objs.length == 0) {
            exists = false;
        }
        else {
            // find the corresponding Try It button and add an event to Correct the textarea
            var formParent = objs[0].parentElement.parentElement.parentElement.parentElement.parentElement;
            for (var i = 0; i < formParent.childNodes.length; i++) {
                if (formParent.childNodes[i].className == "sandbox_header") {
                    var butDiv = formParent.childNodes[i];
                    for (var j = 0; j < butDiv.childNodes.length; j++) {
                        if (butDiv.childNodes[j].className == "submit") {
                            var tryItBut = butDiv.childNodes[j];
                            tryItBut.addEventListener("click", function () {
                                // add the event
                                CorrectTextArea(this, AuthorizationName);
                            }, false);
                        }
                    }
                }
            }
            // rename to AuthorizationName
            objs[0].name = AuthorizationName;
        }
    }
});

function CorrectTextArea(tryItBut, AuthorizationName)
{
    // find the parent of the tryItButton 
    var formParent = tryItBut.parentElement.parentElement;
    // find among it's children the AuthorizationName and correct the content
    var textArea = findName(formParent, AuthorizationName);
    WriteNewAuthorizationStyle(textArea);
    
}

// recursivly find the child with given name
function findName(element, name) {
    var foundElement = null, found;
    function recurse(element, name, found) {
        for (var i = 0; i < element.childNodes.length && !found; i++) {
            var el = element.childNodes[i];
            var elName = el.name;
            if (elName == name) {
                found = true;
                foundElement = element.childNodes[i];
                break;
            }
            if (found)
                break;
            recurse(element.childNodes[i], name, found);
        }
    }
    recurse(element, name, false);
    return foundElement;
}

// recursivly find the child with given Class
function findByClass(element, className) {
    var foundElement = null, found;
    function recurse(element, className, found) {
        for (var i = 0; i < element.childNodes.length && !found; i++) {
            var el = element.childNodes[i];
            var elName = el.className;
            if (elName == className) {
                found = true;
                foundElement = element.childNodes[i];
                break;
            }
            if (found)
                break;
            recurse(element.childNodes[i], className, found);
        }
    }
    recurse(element, className, false);
    return foundElement;
}

function WriteNewAuthorizationStyle(textArea)
{
    textArea.value = textArea.value.replace(/\n/g, " ");
    if (textArea.value.indexOf("InnovationPortal UserID") > -1) {
        // convert just if it is not yet formatted
        return;
    }
    try{
        var obj = JSON.parse(textArea.value);
        // textArea.value = "InnovationPortal UserID=\"" + obj.UserID + "\", SessionToken=\"" + obj.SessionToken + "\", CallerId=\"" + obj.CallerId + "\"";
        textArea.value = "SessionToken=\"" + obj.SessionToken + "\", CallerId=\"" + obj.CallerId + "\"";
    }
    catch(err)
    {
    }
}

function ResizeSwaggerBody()
{
    var bodyObject = document.getElementById("swagger-ui-container");
    bodyObject.setAttribute("style", "max-width:100%;margin-left:15px;margin-right:15px");
}

function HideOptionsMethods()
{
    var optionsObjects = document.getElementsByClassName("options operation");
    
    for (var i = 0; i < optionsObjects.length; i++) {
        optionsObjects[i].setAttribute("style", "display:none");
    }
}

function DoubleClickEvent(textArea)
{
    // first look for output for Login method
    var loginMethod = document.getElementById("Restricted_RESTAPIProfile_Login_content");
    var jsonLoginResponse = findByClass(loginMethod, "json");
    if (jsonLoginResponse != null) {
        var codeElement = jsonLoginResponse.childNodes[0];
        var output = ReadOutput(codeElement);
        if (output != "") {
            textArea.value = output;
            WriteNewAuthorizationStyle(textArea);
        }
        return;
    }
    // else look for output for Register method
    var registerMethod = document.getElementById("Restricted_RESTAPIProfile_Register_content");
    var jsonRegisterResponse = findByClass(registerMethod, "json");
    if (jsonRegisterResponse != null) {
        var codeElement = jsonRegisterResponse.childNodes[0];
        var output = ReadOutput(codeElement);
        if (output != "") {
            textArea.value = output;
            WriteNewAuthorizationStyle(textArea);
        }
        return;
    }
}

// read from the codeNode element all the necessary fields for Credentials
function ReadOutput(codeElement)
{
    var outputText = "{ ";
    var correctResponse = 0; // count if all 3 necessary fields are found
    for (var i = 0; i < codeElement.childNodes.length; i++) {
        var el = codeElement.childNodes[i];
        if (el.innerHTML == undefined || el.innerHTML == "ErrorList")
            continue;

        if (el.innerHTML == "UserID" || el.innerHTML == "SessionToken" || el.innerHTML == "CallerId") {
            outputText += "\"" + el.innerHTML + "\": ";
            correctResponse++;
        }
        else {
            if (el.innerHTML == "ActiveHealth") {
                break;
            }
            outputText += el.innerHTML + ", ";
        }
    }
    if (correctResponse != 3)
    {
        return "";
    }
    // remove the last ,
    outputText = outputText.substring(0, outputText.length - 2) + " }";
    return outputText;
}