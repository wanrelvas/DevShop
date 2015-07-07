
//Model

function DeveloperInfo() {
    this.Username = "";
    this.Price = 0;
    this.Hours = 0;
    this.FinalPrice = 0;
}

//Controller

var DevlopersList = [];

function GetDeveloperDataFromInputs() {
    var userName = $("#catalogTableUsername").val();
    var price = $("#catalogTablePrice").val();
    var hours = $("#catalogTableHours").val();

    var developerInfo = new DeveloperInfo();
    developerInfo.Username = userName;
    developerInfo.Price = parseFloat(price);
    developerInfo.Hours = parseInt(hours);    

    return developerInfo;
}

function AddDevloperToCart(devInfo) {

    var existentItem = false;

    $.each(DevlopersList, function (i, developer) {
        if (developer.Username == devInfo.Username) {
            existentItem = true;
            error = "<div class=\"col-lg-6 alert alert-danger\" role=\"alert\"> <strong>Developer was not added.</strong> This developer username is already on cart.</div>";

            $('#StoreCatalogTable > .row > div:last').after(error);
        }
    });

    if (!existentItem) {

        $.ajax({
            url: '/api/ShoppingCart/',
            type: 'POST',
            data: JSON.stringify(devInfo),
            contentType: "application/json;charset=utf-8",
            success: function (developer) {
                ClearInputs();
                AddDeveloperToCartTable(developer);
                DevlopersList.push(developer);
                UpdateCartTableTotal();
            },
            error: function (data) {
                error = "<div class=\"col-lg-6 alert alert-danger\" role=\"alert\"> <strong>Developer was not added.</strong> This developer username is already on cart.</div>";
                $('#StoreCatalogTable > .row > div:last').after(error);
            }
        });

        
    }
}

function AddDeveloperToCartTable(developerInfo) {

    $(function () {
        newCartItem = "<tr>" +
            "<td class=\"username\" >" +
           developerInfo.Username +
           "</td> <td  class=\"price\">" +
           "$" +
           developerInfo.TotalPrice.toString() +
            "</td> <td>" +
           "<button type=\"button\" class=\"btn btn-danger\"  onclick=\"RemoveCartItem(this)\">Remove</button>" +
        "</td> </tr>";

        $('#StoreShoppingCartTable > tbody > tr:last').after(newCartItem);

        $( "#finishOrder").prop( "disabled", false );
    });

}

function UpdateCartTableTotal() {
    var total = 0;

    DevlopersList.forEach(function (devInfo) {
        total += devInfo.TotalPrice;
    });

    $(".totalPrice").text("$" + total.toString());
}

function ClearInputs() {
    $("#catalogTableUsername").val("");
    $("#catalogTablePrice").val("");
    $("#catalogTableHours").val("");
}

function LoadCartTable() {

    $.getJSON('/api/ShoppingCart', function (shoppingCart) {
        if (shoppingCart != null) {
            var developers = shoppingCart.ShoppingCartDevelopers;
        }

        $(developers).each(function (i, developer) {
            AddDeveloperToCartTable(developer);
            DevlopersList.push(developer);
        });

        UpdateCartTableTotal();
    });

}

function RemoveCartItem(button) {

    var $tableRow = $(button).closest("tr");

    var $tableRowUsernameCol = $tableRow.find(".username").text();

    var $index = $tableRow.index();
   

    $.ajax({
        url: '/api/ShoppingCart/' + $tableRowUsernameCol.toString(),
        type: 'DELETE',
        success: function (data) {
            DevlopersList.splice($index - 1, 1);
            UpdateCartTableTotal();
            $tableRow.remove();
            if (DevlopersList.length = 0) {
                $("#finishOrder").prop("disabled", true);
            }
        },
        error: function (data) {
            error = "<div class=\"col-lg-6 alert alert-danger\" role=\"alert\"> <strong>Developer was not found on server.</strong></div>";
            $('#StoreCatalogTable > .row > div:last').after(error);           
        }
    });
    
}

function FinishOrder() {


    $.ajax({
        url: '/api/ShoppingCart',
        type: 'DELETE',
        success: function (data) {
            var url = "./Home/CheckOut";
            $(location).attr('href', url);

        },
        error: function (data) {
            error = "<div class=\"col-lg-6 alert alert-danger\" role=\"alert\"> <strong>It was not possible to finish ordering.</strong> Unexpected sever error ocurred. </div>";
            $('#StoreCatalogTable > .row > div:last').after(error);
        }
    });
       
}


function LoadDeveloperPrice() {

    var username = $("#catalogTableUsername").val();

    $.getJSON('/api/ShoppingCart/' + username,
        function (shoppingCartDeveloper) {
            if (shoppingCartDeveloper != null) {
                var devPrice = shoppingCartDeveloper.Price;
                $("#catalogTablePrice").val(devPrice);
            }
            else {
                error = "<div class=\"col-lg-6 alert alert-danger\" role=\"alert\"> <strong>Developer was not found on server.</strong> The developer was not found on GitHub. Try another username to contiue.</div>";
                $('#StoreCatalogTable > .row > div:last').after(error);
            }

            
          });

}


//Handlers
$(document).ready(function () {

    $("#finishOrder").prop("disabled", true);
    $("#catalogTablePrice").prop("disabled", true);

    LoadCartTable();

    $("#addDevloperBtn").click(function () {
        AddDevloperToCart(GetDeveloperDataFromInputs());
    });

    $("#finishOrder").click(function () {
        FinishOrder();
    });

    $("#catalogTableUsername").focus(function () {
        $("div[role='alert']").remove();
    });

    $("#catalogTableUsername").focusout(function () {
        LoadDeveloperPrice();
    });
});

