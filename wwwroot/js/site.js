// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function showMessage(messageType, message) {
    var messageContainer = document.getElementById("message-container");

    if (messageContainer) {
        // Create a message element
        var messageElement = document.createElement("div");
        messageElement.className = "message " + messageType;
        messageElement.innerText = message;

        // Append the message element to the message container
        messageContainer.appendChild(messageElement);

        // Display the message container
        messageContainer.style.display = "flex";

        // Automatically remove the message after 5 seconds
        setTimeout(function () {
            messageElement.remove();

            // Hide the message container if it's empty
            if (messageContainer.childElementCount === 0) {
                messageContainer.style.display = "none";
            }
        }, 2000); // Show for 5 seconds
    }
}

function getUserSessionInfo() {
    $.ajax({
        url: "/Session/GetUserSessionInfo",
        success: function (data) {
            $('#sessionUserId').val(data)
        }
    });
}



function encryptElement(idOfElement) {
    var sessionId = $('#sessionUserId').val();
    if (sessionId == '') {
        getUserSessionInfo();
        sessionId = $('#sessionUserId').val();
    }
    var desiredLength = 16;
    if (sessionId !== '') {
        sessionId = sessionId.replace(/[^0-9a-zA-Z]/g, '')
        if (sessionId.length < desiredLength) {
            while (sessionId.length < desiredLength) {
                sessionId += '0';
            }
        } else if (sessionId.length > desiredLength) {
            sessionId = sessionId.substring(0, desiredLength);
        }
        var key = CryptoJS.enc.Utf8.parse(sessionId);
        var iv = CryptoJS.enc.Utf8.parse(sessionId);
        const userInput = $('#' + idOfElement).val();
        if (userInput !== '') {
            var encryptedText = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(userInput), key,
                {
                    iv: iv,
                    mode: CryptoJS.mode.CBC,
                    padding: CryptoJS.pad.Pkcs7
                });  
            document.getElementById(idOfElement).value = encryptedText;
        }   
        
    }
}
