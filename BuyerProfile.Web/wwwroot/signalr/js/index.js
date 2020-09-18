var chatterName = 'Visitor';
var myTimeOut;
var Questions;
var formId = '';
var dialogEl = document.getElementById('chatDialog');
var questionAnswers = [];
// Initialize the SignalR client
var connection = new signalR.HubConnectionBuilder()
    .withUrl('http://localhost:56594/chatHub')
    .build();



// get messages
connection.on('ReceiveMessage', renderMessage);

//get room Id
connection.on('PassRoomId', PassId);

//show survey form after run timeout
connection.on('RunTimeOut', runSetTimeOut);

//get question list from db
connection.on('QuestionList', QuestionsValues)

// create survey form as html
connection.on('GenerateFormId', SetFormId)

connection.onclose(function () {
    onDisconnected();
    setTimeout(startConnection, 5000);
})

function startConnection() {
    connection.start()
        .then(onConnected)
        .catch(function (err) {
            console.error(err);
        });
}
function runSetTimeOut() {
    clearTimeout(myTimeOut);
    //myTimeOut = window.setTimeout(function () {
    //    connection.invoke('GetQuestions');
    //}, 15000);

}
function QuestionsValues(values, pageIndex = 0) {
    $("#bottomPanel").hide();
    Questions = values.result;
    SetFormValues(Questions[pageIndex], pageIndex);

}
function SetFormId(frmId) {
    formId = frmId;

}

// initialize survery form
function SetFormValues(question, pageIndex) {
    var myForm = '';

    if (parseInt(pageIndex) > Questions.length - 1) {
        myForm = '<p>Thank you for store vote !</p>'; // Pressed done btn
        AssignedForm(myForm);
        return;
    }

    myForm = `<div id="Survey"  style="border-style:groove;border-radius: 5px;padding: 5px;">
    <input type="hidden" id="QuestionId" value="`+ question.id + `" />
    <p>`+ question.questionTitle + `</p>`;
    if (questionAnswers.length > 0) {
        var selectedAnswer = questionAnswers.filter(function (index) {
            return index.id == question.id;
        })
        var check = "checked";
        console.log(selectedAnswer);
        if (selectedAnswer.length > 0) {
            if (selectedAnswer[0].answer == 1) {
                myForm += `<input type="radio" name="option" ` + check + ` value='1' /> ` + question.firstOption + `
                    <br />`;
            }
            else {
                myForm += `<input type="radio" name="option" value='1' /> ` + question.firstOption + `
                    <br />`;
            }

            if (selectedAnswer[0].answer == 2) {
                myForm += `<input type="radio" name="option" ` + check + ` value='2' /> ` + question.secondOption + `
                    <br />`;
            } else {
                myForm += `<input type="radio" name="option" value='2' /> ` + question.secondOption + `
                    <br />`;
            }

            if (selectedAnswer[0].answer == 3) {
                myForm += `<input type="radio" name="option" ` + check + ` value='3' /> ` + question.thirdOption + `
                    <br />`;
            } else {
                myForm += `<input type="radio" name="option" value='3' /> ` + question.thirdOption + `
                    <br />`;
            }

            if (selectedAnswer[0].answer == 4) {
                myForm += `<input type="radio" name="option" ` + check + ` value='4' /> ` + question.fourthOption + `
                    <br />`;
            }
            else {
                myForm += `<input type="radio" name="option"  value='4' /> ` + question.fourthOption + `
                    <br />`;
            }
        }
        else {
            myForm += `<input type="radio" name="option"  value='1' /> ` + question.firstOption + `
                        <br />
                        <input type="radio" name="option"  value='2' /> ` + question.secondOption + `
                        <br />
                        <input type="radio" name="option"  value='3' /> ` + question.thirdOption + `
                        <br />
                        <input type="radio" name="option"  value='4' /> ` + question.fourthOption + `
                        <br />`;
        }

    }
    else {
        myForm += `<input type="radio" name="option" value='1' /> ` + question.firstOption + `
                    <br />
                    <input type="radio" name="option" value='2' /> ` + question.secondOption + `
                    <br />
                    <input type="radio" name="option" value='3' /> ` + question.thirdOption + `
                    <br />
                    <input type="radio" name="option" value='4' /> ` + question.fourthOption + `
                    <br />`;
    }

    if (parseInt(pageIndex) <= 0) {
        myForm += `<button onclick="NextFunc(\`` + "1" + `\`)">Next</button>`
    }
    else if (parseInt(pageIndex) == Questions.length - 1) {
        myForm += `<button onclick="NextFunc(` + (parseInt(pageIndex) + 1) + `)">Done</button>
        <button onclick="PreviousFunc(` + (parseInt(pageIndex) - 1) + `)">Previous</button>`;

    }
    else {
        myForm += `<button onclick="NextFunc(` + (parseInt(pageIndex) + 1) + `)">Next</button>
        <button onclick="PreviousFunc(` + (parseInt(pageIndex) - 1) + `)">Previous</button>`;
    }
    myForm += `</div >`;


    AssignedForm(myForm);


}

// create chat history
function AssignedForm(myForm) {
    var messageDiv = document.createElement('div');
    messageDiv.className = 'message';
    messageDiv.innerHTML = myForm;

    var newItem = document.createElement('li');
    newItem.appendChild(messageDiv);
    $("#Survey").remove();
    var chatHistoryEl = document.getElementById('chatHistory');
    chatHistoryEl.appendChild(newItem);
    chatHistoryEl.scrollTop = chatHistoryEl.scrollHeight - chatHistoryEl.clientHeight;
}
// call next btn func in survey form
function NextFunc(pageIndex) {

    var optionChecked = $('input[name="option"]:checked').val();


    if (optionChecked == 'undefined' || optionChecked == null) {
        alert('please select your option!'); return;
    }
    $.ajax({
        "url": "http://localhost:56594/Home/SetVote",
        "type": "POST",
        "datatype": "json",
        "data": {
            id: $("#QuestionId").val(), answer: optionChecked, formid: formId, userId: $("#UserId").val() },
        success: function (res) {
            if (res.success) {
                questionAnswers = jQuery.grep(questionAnswers, function (value) {
                    return value.id != $("#QuestionId").val();// pop duplicate value from array
                });
                var answers = {
                    id: $("#QuestionId").val(),
                    answer: parseInt(optionChecked)
                }

                questionAnswers.push(answers);
                SetFormValues(Questions[pageIndex], pageIndex);
            }
            else
                alert('error in submit vote!');
        },
        error: function (msg) {
            alert('error in submit vote!');
        }

    });
}
function openfile(a) {
    $(a).trigger('click');
}
// attach file

function UpFile() {
    // Checking whether FormData is available in browser  
    if (window.FormData !== undefined) {

        var fileUpload = $("#file-input").get(0);
        var files = fileUpload.files;

        // Create FormData object  
        var fileData = new FormData();
        fileData.append("RoomId", $("#roomId").val());
        fileData.append("senderName", $("#UserName").val());
        // Looping over all files and add it to FormData object  
        for (var i = 0; i < files.length; i++) {
            fileData.append(files[i].name, files[i]);
        }

        $.ajax({
            url: 'http://localhost:56594/Home/GetFile',
            type: "POST",
            contentType: false, // Not to set any content header  
            processData: false, // Not to process data  
            data: fileData,
            success: function (result) {
                console.log(result);
            },
            error: function (err) {
                console.log(err.statusText);
            }
        });
    } else {
        alert("FormData is not supported.");
    }
}

// call Previous btn func in survey form

function PreviousFunc(pageIndex) {
    SetFormValues(Questions[pageIndex], pageIndex);
}

function onDisconnected() {
    dialogEl.classList.add('disconnected');
}

function onConnected() {
    dialogEl.classList.remove('disconnected');

    var messageTextboxEl = document.getElementById('messageTextbox');
    messageTextboxEl.focus();

    connection.invoke('SetName', chatterName);
    connection.invoke('GetRoomId');
    clearTimeout(myTimeOut);
    myTimeOut = window.setTimeout(function () {
        connection.invoke('GetQuestions');
    }, 2000000);
}



function showChatDialog() {
    dialogEl.style.display = 'block';
}

function sendMessage(text) {
    if (text && text.length) {
        connection.invoke('SendMessage', chatterName, text);
    }
}
//set room id in hidden input
function PassId(roomId) {
    $("#roomId").val(roomId);
}

function ready() {
    setTimeout(showChatDialog, 750);

    var chatFormEl = document.getElementById('chatForm');
    chatFormEl.addEventListener('submit', function (e) {
        e.preventDefault();
        var text = e.target[0].value;
        e.target[0].value = '';
        sendMessage(text);
    });


    var welcomePanelEl = document.getElementById('chatWelcomePanel');
    welcomePanelEl.addEventListener('submit', function (e) {
        e.preventDefault();

        var name = e.target[0].value;
        if (name && name.length) {
            welcomePanelEl.style.display = 'none';
            chatterName = name;
            startConnection();
        }
    });
}
// if win focus has false show notif
function CheckWinFocus() {
    if (document.hasFocus()) return true;
    else return false;

}

// generate notification
function GetNotification(name, time, message) {
    Push.create(`Message from` + name, {
        body: name + ' ' + time + "\n" + message,
        data: "SinjulMSBH",
        icon: '',
        link: "",
        tag: '',
        requireInteraction: true,
        timeout: 5000,
        vibrate: [200, 100],
        silent: false,
        onClick: function () {
            window.focus();
        },
        onClose: function () {
            this.close();
        },
        onError: function () {
            console.log('onError in notification!');
        },
        onShow: function () {
        },
    });
}

// generate incomming or out message
function renderMessage(name, time, message) {
    if (!CheckWinFocus())
        GetNotification(name, time, message);

    var nameSpan = document.createElement('span');
    nameSpan.className = 'name';
    nameSpan.textContent = name;

    var timeSpan = document.createElement('span');
    timeSpan.className = 'time';
    var friendlyTime = moment(time).format('H:mm');
    timeSpan.textContent = friendlyTime;

    var headerDiv = document.createElement('div');
    headerDiv.appendChild(nameSpan);
    headerDiv.appendChild(timeSpan);

    var messageDiv = document.createElement('div');
    messageDiv.className = 'message';
    messageDiv.innerHTML = message;

    var newItem = document.createElement('li');
    newItem.appendChild(headerDiv);
    newItem.appendChild(messageDiv);

    var chatHistoryEl = document.getElementById('chatHistory');
    chatHistoryEl.appendChild(newItem);
    chatHistoryEl.scrollTop = chatHistoryEl.scrollHeight - chatHistoryEl.clientHeight;
}

document.addEventListener('DOMContentLoaded', ready);