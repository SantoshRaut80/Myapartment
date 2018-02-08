


        
var minvalue = 0;
var maxvalue = 10;
var ImageDictionary = {};
        

/// ********************************Load function 
$(document).ajaxStart(function () {
    //   $('.dvLoading_first').show();
}).ajaxStop(function () {
    // $('.dvLoading_first').hide();
});

        

$(document).ready(function () {
  
    window.parent.FrameSourceChanged();

    sessionStorage.setItem("minvalue", 0);
    //  alert("document ready");
    var containerHeight = "440px";
    $(window).height("500px");
    $("#view_div").height("500px");
});

$(document).bind("beforeunload", function () {
    return confirm("Do you really want to close?");
})

      

// ************************************  Date  Change format function 
function ChangeDateformat(inputdate)
{
    var NewFormatDate;
    var date = new Date(inputdate);

    //alert(date);
    var Currentdate = new Date();

    var DateFormat = date.toLocaleDateString();

    var TimeFormat = date.toLocaleTimeString();

    var CurrentDateFormat = Currentdate.toLocaleDateString();


    var Hoursdiff = Math.ceil(Currentdate.getTime() - date.getTime()) / 3600000;
         
    var diff = (Currentdate.getTime() - date.getTime()) / 1000;
    diff /= 60;

    //   alert(Currentdate.getTime() - date.getTime());
           
    var Minsdiffnew = Math.abs(Math.round(diff));

    //  alert(diff);
    //   alert(Minsdiffnew);

    var TimeHoursdiff = Math.floor(Hoursdiff);         
           
    if (TimeHoursdiff < 12)
    {
        if (TimeHoursdiff < 1)
        {
            if (Minsdiffnew == 0)
            {
                NewFormatDate = "just now";
            }
            else {

                NewFormatDate = Minsdiffnew + "min";
            }                  
        }
        else
        {
            NewFormatDate = TimeHoursdiff + "hrs";
        }            
    }

    else {
        NewFormatDate = DateFormat + "  " + TimeFormat;
    }

            

    return NewFormatDate;          
}


var lastScrollTop = 0;
var count = 1;
/////////******************scrolling  function **************////////////////////
jQuery(function ($) {
    $('#scroll_div').bind('scroll', function () {
               
        if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight) {

            st = $(this).scrollTop();
            if (st < lastScrollTop) {
                           
            }
            else {
                           
                var sessionminval = sessionStorage.getItem("minvalue");
                            
                if (sessionminval != null) {
                    if (sessionminval == minvalue+10)
                    {
                                    
                        $("#status").text("scroll " + count);
                        count++;
                        minvalue = minvalue + 10;
                        maxvalue = minvalue + 10;
                        // Service(minvalue, maxvalue);
                        GetForumData(minvalue, maxvalue);
                    }
                    else if (minvalue + 10 > sessionminval)
                    {
                                     
                    }
                              
                }
                else {
                               
                    minvalue = 0;
                    maxvalue = 10;
                    // Service(minvalue, maxvalue);
                    GetForumData(minvalue, maxvalue);
                }

                            
            }
            lastScrollTop = st;
                      
        }

               
    })
}
);


window.onload = function GetData() {

    //alert("Window On Load");
    sessionStorage.clear();
    GetForumData(minvalue, maxvalue);
    //Service(minvalue, maxvalue);

    document.getElementById("New_Discussing").src = "AppImage/Userimages/" + ResiID + ".png";
}
        
function GetForumData(minvalue, maxvalue) {
            

    $.ajax({
        type: "POST",
        url: "Discussions.aspx/GetForumData",
        data: '{StartIndex:' + minvalue + ',EndIndex:' + maxvalue +'}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnSuccess,
        failure: function (response) {
            alert(response.d);
            sessionStorage.clear();
        }
    });
}
function OnSuccess(response) {
            

    // var da = JSON.stringify(data);
    var js = jQuery.parseJSON(response.d);
    //var jarray = js.$values;
    if (js.length > 0) {
        var newmin = minvalue + js.length
        Data(js);
        sessionStorage.setItem("minvalue", minvalue + js.length);
    }
    else {
        //alert("NoData")
    }

}

function Service(minvalue, maxvalue) {
    var url = "http://www.kevintech.in/" + SocietyName + "/api/ForumDiff";
    var reqBody = "{\"StartIndex\":" + minvalue + ",\"EndIndex\":" + maxvalue  +",\"LastRefreshTime\":\"\"}";
    $.ajax({
        dataType: "json",
        url: url,
        data: reqBody,
        type: 'post',
        contentType: 'application/json',
        success: function (data)
        {
            var da = JSON.stringify(data);
            var js = jQuery.parseJSON(da);
            var jarray = js.$values;
            if (jarray.length > 0) {
                var newmin = minvalue + jarray.length
                Data(jarray);
                sessionStorage.setItem("minvalue", minvalue+jarray.length);
            }
            else {
                //alert("NoData")
            }
            // alert("sucess");
            // alert("Storing min value in session is " + minvalue);
                   
        },
        error: function (data, errorThrown) {
            alert('request failed :' + errorThrown);
            sessionStorage.clear();
        }

    });

           
    //  sessionStorage.setItem("maxvalue", maxvalue);
}

//Onload  function  for  discussions
function Data(jarray) {

    var viewString="";
    // var table = document.getElementById("view_table");

    var viewDiv = document.getElementById("view_div");

    var length = jarray.length;

    for (var i = 0; i < length; i++) {
        var RowId = +minvalue + +i;
        var ThreadDate = jarray[i].InitiatedAt;
        var UpdatedDate = jarray[i].UpdatedAt;
        var resID = jarray[i].FirstResID;
        var lastResID = jarray[i].LastResID;

        if (ImageDictionary[resID] == null) {
            ImageDictionary[resID] = "";
            GetImage(resID);
        }

        if ((lastResID != resID) && (ImageDictionary[lastResID] == null)) {
            ImageDictionary[lastResID] = "";
            GetImage(lastResID);
        }

        viewString += '</br><table class="forum_table">';
        viewString += "<tr><td colspan=\"4\" style=\"height:5px;background-color: #e8e6e6; border-radius:10px;\"></td></tr>"
        viewString += SummaryRow(i, jarray, RowId);
               
        if (ThreadDate != UpdatedDate) {
                    
            //var row = document.getElementById("Summary" + RowId).parentElement;
            // alert(row.innerHTML);
            viewString += LastestCommentRow(i, RowId, jarray, length);
            viewString += "<tr><td colspan=\"4\" style=\"height:20px;\"></td></tr>"
                   
        }
        else {

            viewString += "<tr><td colspan=\"4\" style=\"height:20px;\"></td></tr>"
        }

        viewString += '</table>';
               
        viewDiv.innerHTML += viewString;
        viewString = ""
    }
}

//Give  min  size  to  textbox 
function GetCount(textbox) {

    var Text = document.getElementById("New_Notification_Text");
    var button = document.getElementById("Discuss_Submit");
    var size = 500 - Text.value.length;
          
    $('#lblTextSize').text(size + ' Character');
            
    if (Text.value.length < 20) {
        // alert("Post  text Should be greaterthan 20 characters");
        button.disabled = true;
    }

    else {
        button.disabled = false;
        //do nothing
    }
}


// new discussion  post  function
var Text="";
function DiscussionDataSend() {
         
           
    if (Text == "") {
        Text = document.getElementById("New_Notification_Text").value;
        if (Text.length < 20) {
            return;
        }
    }
    else {
              
    }
          
    document.getElementById("post_loading").style.display = "block";
           
    var strTopic = "General";
         
    Text.replace(/"/g, '\\"');
    var ThreadID = 0;
           
    var url = "http://www.kevintech.in/" + SocietyName + "/api/forum";
            
    var reqBody = "{\"resID\":\"" + ResiID + "\",\"ThreadID\":\"" + ThreadID + "\", \"Topic\":\"" + strTopic + "\",\"CurrentThread\":\"" + Text + "\"}"
           
    setTimeout(function () {
        $.ajax({
            dataType: "json",
            url: url,
            async: true,
            data: reqBody,
            type: 'post',
            async: false,
            contentType: 'application/json',
            success: function (data) {
                       
                location.reload();
            },
            error: function (data, errorThrown) {
                document.getElementById("post_loading").style.display = "none";
                // document.getElementById("New_Notification_Text").value = "";
                Text = "";
                alert('Error Updating comment, try again');
            }
        });
    }, 0);
}


        

//Reply To a Thread

function ReplyForThread(Topic, ThreadID, id) {
    var Text = document.getElementById("Add_Comment_txtbox " + id + "").value;

    var url = "http://www.kevintech.in/" + SocietyName + "/api/forum";

    var reqBody = "{\"resID\":\"" + ResiID + "\", \"ThreadID\":\"" + ThreadID + "\",\"Topic\":\"" + Topic + "\",\"CurrentThread\":\"" + Text + "\"}"
    // alert(1);
    document.getElementById("reply_loading").style.display = "inline";
    //$('#reply_loading').show();
    $('.new_comment_row').attr('opacity', 0.5);

    setTimeout(function () {
        $.ajax({
            dataType: "json",
            url: url,
            data: reqBody,
            type: 'post',
            async: false,
            contentType: 'application/json',
            success: function (data) {
                location.reload();
            },
            error: function () {
                alert('Error Updating comment, try again');
            }
        });
    }, 0);

}


       


//Discussion summary function 
function LoadData(RowId, ThreadID, ThreadDate, Comments) {
        
    $(".comment_row").remove();
    $(".collapse_row").remove();
                    
    sessionStorage.clear();                                // clear the session 
         
    var table = document.getElementById("view_table");
    var row = document.getElementById("Summary" + RowId).parentElement;   // create  detaild summary(comments)  for  the discussion 

      
          
    var url = "http://www.kevintech.in/" + SocietyName + "/api/forum/" + ThreadID;

    $.get(url, function GetThreadID(data) {
        // alert("working");
        var ThreadData = JSON.stringify(data);
        //alert(ThreadData);
        var ThrJson = jQuery.parseJSON(ThreadData);
        //   alert(ThrJson);
        var Tjarray = ThrJson.$values;
        //  alert(Tjarray);


        DisplayThreadData(Tjarray, RowId, ThreadID, ThreadDate, Comments);
               
    });
}


function DisplayThreadData(Tjarray, RowId, ThreadID, ThreadDate, Comments) {
           
    var length = Tjarray.length-1;
    // alert(length);
    if (Comments > 1) {
               
        for (var i = 0; i < length; i++) {
            var resID = Tjarray[i].ResID;
            if (ImageDictionary[resID] == null) {
                ImageDictionary[resID] = "";
                GetImage(resID);
            }

            var row = document.getElementById("Summary" + RowId).parentElement;
                   
            var CommentThreadDate = Tjarray[i].UpdatedOn;
            var Newformat = ChangeDateformat(CommentThreadDate);
            if (Newformat != ThreadDate)
            {
                row.innerHTML += CommentRow(i, RowId, Tjarray, length, CommentThreadDate);
            }
                   
        }
        row.innerHTML += '<tr class="collapse_row" style="background-color:#ECEFF1;"><td id="Collapse' + RowId + '" colspan="6" style=\"padding-left:2%;\"><a id="Collapse" class=\"fa fa-angle-up link_btn\" onclick="Collapse( ' + RowId + ',' + length + ')"  href= "javascript:void(0)"></a></td></tr>';
             
    }
}

function SummaryRow(id, jarray, RowId) {

    var Topic = jarray[id].Topic;         
    var date = jarray[id].InitiatedAt;           
    var Newformat = ChangeDateformat(date);
    var Comments = (jarray[id].commentsCount - 1);
    var FirstImageSource =LastImageSource = "Images/Profile.jpg";
       
    if (ImageDictionary[jarray[id].FirstResID] != null)
    {
        FirstImageSource = "data:image/jpeg;charset=utf-8;base64," + ImageDictionary[jarray[id].FirstResID];
    }
    if (ImageDictionary[jarray[id].LastResID] != null)
    {
        LastImageSource = "data:image/jpeg;charset=utf-8;base64," + ImageDictionary[jarray[id].LastResID];
    }
                
    //  var strSummaryRow = "<tr ><td colspan=\"5\" style=\"height:5px;background-color:white;\"></td></tr>" +

    var strSummaryRow =    '<tr id="Summary' + RowId + '" class="main_thread_row">' +
                           '<td class="image_div" >' +
                           '<img class="img_' + jarray[id].FirstResID + '" src=' + FirstImageSource + ' onclick="DisplayFullImage(this)"  />' +
                           '<div class="user_div">' + jarray[id].Initiater + ' from ' + jarray[id].firstFlat + '</div>' +
                           '</td>' +
                                  
                          '<td colspan="2" class="main_text">' + jarray[id].FirstThread + '<p class="td_date">' + Newformat + '</p>  </td>' +
                        '<td class="topic_dv"> </td>'+
                        '</tr>' +
                           '<tr>' +
                                   '<td class="image_div">' +
                                    '<td class="image_div">' +
                                  "<td class=\"icon_td\"><a href=\"javascript:void(0)\" class=\"fa fa-comment-o link_btn\" onclick=\"LoadData(" + RowId + ",'" + jarray[id].ThreadID + "','" + Newformat + "'," + Comments + ")\"></a> <span style=\"margin-left:5%;\"></span><a href=\"javascript:void(0)\" id=\"Add_comments" + RowId + "\"  class=\"fa fa-reply link_btn\" onclick=\"AddComment('" + Topic + "'," + RowId + "," + jarray[id].ThreadID + ");\"> </a> <span style=\"margin-left:5%;\"> " + Comments + " comments</td>" +
                                "<td class=\"topic_dv\"> </td>" +
                           "</tr> "
          
    return strSummaryRow;
}

function CommentRow(id, RowId, Tjarray, length, date) {
           
    var date = Tjarray[id].UpdatedOn;
    var Newformat = ChangeDateformat(date);       
    var LastImageSource = "Images/Profile.jpg";
            
    if (ImageDictionary[Tjarray[id].ResID] != null) {
        LastImageSource = "data:image/jpeg;charset=utf-8;base64," + ImageDictionary[Tjarray[id].ResID];
    }
    var strCommentRow = '<tr id="Comment' + id + '" class="comment_row">' +
                             '<td class="image_div">' +
                             "</td> " +
                             '<td class="image_div">' +
                                '<img class="img_' + Tjarray[id].ResID + '" src="' + LastImageSource + '" onclick="DisplayFullImage(this)"  />' +
                                '<div class=\"user_div\">' + Tjarray[id].FirstName + '  from  ' + Tjarray[id].FlatNumber + '</div>' +
                             '</td>'+

                         '<td colspan=\"2\"  class=\"td_Comment_style\">' + Tjarray[id].Thread + '<p class=\"td_date\">' + Newformat + '</p>  </td>' +

                    '<td class=\"topic_dv\"> </td>' +

                    '</tr>';
              
    return strCommentRow;
}
       
        

function LastestCommentRow(id, RowId, Tjarray, length, ThreadDate) {
           
    var date = Tjarray[id].UpdatedAt;
    var Newformat = ChangeDateformat(date);
    var LastImageSource = "Images/Profile.jpg";

          

    if (ImageDictionary[Tjarray[id].LastResID] != null) {
        LastImageSource = "data:image/jpeg;charset=utf-8;base64," + ImageDictionary[Tjarray[id].LastResID];
    }

    var strCommentRow = '<tr class="lastest_comment_row"  id="LatestComment' + RowId + '">' +
                             '<td class="image_div"> </td>' +

                               '<td class="image_div" >' +
                                  '<img class="img_' + Tjarray[id].LastResID + '" onclick="DisplayFullImage(this)" src="' + LastImageSource + '"/>' +
                                  "<div class=\"user_div\"> " + Tjarray[id].CurrentPostBy + "  from  " + Tjarray[id].lastFlat + "</div>" +
                              " </td>" +
                              "<td class=\"last_comment_text \">" + Tjarray[id].latestThread +  "<p class=\"td_date\">" + Newformat + " </p></td>" +
                               "<td class=\"topic_dv\"> </td>" +
                            "<tr/>";
                                 
    return strCommentRow;
            
}

function AddComment(Topic, id, ThreadID) {
    // alert(1);
    $('.new_comment_row').remove();

    var table = document.getElementById("view_table");

    if (table.rows["AddComment" + id]) {

    }

    else {

        var row = document.getElementById("Summary" + id).parentElement;
               
        row.innerHTML += "<tr id=\"AddComment" + id + "\" class=\"new_comment_row\"> " +
            "<td class=\"image_div\"></td>" +
            "<td class=\"image_div\"></td>" +
            "<td class=\"new_Comment_td\"><input type=\"text\" id=\"Add_Comment_txtbox " + id + "\" class=\"Comment_Textbox\" onchange=\"Discuss_Comment()\"   placeholder=\"Write a Comment..\"/><img id=\"reply_loading\" src=\"Images/ajax-loader.gif\" style=\"margin-left:25%;\"/>  </td> " +
            "<td style=\"border-bottom:1px solid #bfbfbf;text-align:left;\">  <button type=\"submit\" id=\"Discuss_Comment_Submit\" onclick=\"ReplyForThread( '" + Topic + "'," + ThreadID + "," + id + ")\"  class=\"button\" >Submit</button></td> " +
            "</tr>   ";
    }

}

  

function Collapse(id, length)   // collapse function 
{

    $(".comment_row").remove();
    $(".collapse_row").remove();
    sessionStorage.clear();
}

function GetImage(ResID)
{

    var url = "http://www.kevintech.in/" + SocietyName + "/api/Image/GetByResID/" + ResID + "/";
    alert(url);
  //  alert(FlatNumber);
    // var url = "Discussions.aspx/GetByResID?ResID=" + ResID;
    $.ajax({
        dataType: "json",
        url: url,
                
        success: function (data) {
                    
            if (data != null) {
                var da = JSON.stringify(data);
                var js = jQuery.parseJSON(da);
                var ImageStr = js.ImageString;
                //alert(ImageStr);
                ImageDictionary[ResID] = ImageStr;
                var Source = "data:image/jpeg;charset=utf-8;base64," + ImageStr;
                var cla = "img_" + ResID;
                        
                var x = document.getElementsByClassName(cla);
                var i;
                      
                for (i = 0; i < x.length; i++) {
                    x[i].src = Source;
                }

                    
            }
            else {
                       
                document.getElementById("lblMessage").textContent = "Email is available"
                document.getElementById("inAddTEmailID").disabled = false;
            }

        },
        error: function (data, errorThrown) {
            // alert("Error getting Image " + errorThrown);
                   
            var x = document.getElementsByClassName("img_" + ResID);
            for (var i = 0; i < x.length; i++) {
                x[i].src = "Default_Icon/profile.jpg";
            }
        }

    });
}

function DisplayFullImage(ctrlimg) {
    txtCode = "<HTML><HEAD>"
    + "</HEAD><BODY TOPMARGIN=0 LEFTMARGIN=0 MARGINHEIGHT=0 MARGINWIDTH=0><CENTER>"
    + "<IMG src='" + ctrlimg.src + "' BORDER=0 NAME=FullImage "
    + "onload='window.resizeTo(document.FullImage.width,document.FullImage.height)'>"
    + "</CENTER>"
    + "</BODY></HTML>";
    mywindow = window.open('', 'image', 'toolbar=0,location=0,menuBar=0,scrollbars=0,resizable=0,width=1,height=1');
    mywindow.document.open();
    mywindow.document.write(txtCode);
    mywindow.document.close();
}

function auto_grow(element) {

    element.style.height = "5px";
    element.style.height = (element.scrollHeight -20) + "px";
}

