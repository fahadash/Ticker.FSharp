/// <reference path="C:\Dev\community\Ticker.FSharp\Samples\SignalR.Ticker.FSharp\Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="C:\Dev\community\Ticker.FSharp\Samples\SignalR.Ticker.FSharp\Scripts/jquery.signalR-2.2.0.js" />

// A simple background color flash effect that uses jQuery Color plugin
jQuery.fn.flash = function(color, duration, easeduration) {
    var current = this.css('backgroundColor');
    this.animate({ backgroundColor: 'rgb(' + color + ')' }, duration)
        .animate({ backgroundColor: current }, easeduration);
};


$(function () {
    var hub = $.connection.tickerFsharp, $shape = $("#shape");

    var $stockTable = $("#stockTable");

    var $stockTableBody = $stockTable.find('tbody');
    
    var rowTemplate = '<tr datasymbol="{Symbol}"><td>{Symbol}</td><td>{Price}</td><td>{LastUpdated}</td></tr>';

    hub.client.tick = function (symbol, price, lastUpdated, up) {

        $("tr.loading").hide();
        var row = $stockTableBody.find("tr[datasymbol=\"" + symbol + "\"]");
        var d = new Date(lastUpdated);

        var minutes = "00" + d.getMinutes().toString();
        minutes = minutes.substr(minutes.length - 2, 2);

        var time = d.getHours().toString() + ":" + minutes;

        if (row.length == 0) {
        	var templatedRow = rowTemplate
				/// Replace all occurances of {Symbol}
				.replace(new RegExp("\{Symbol\}", "g"), symbol)
                .replace("{Price}", price)
                .replace("{LastUpdated}", time);
            $stockTableBody.append(templatedRow);
        } else {

            var cells = row.first().find("td");

            cells.eq(0).html(symbol);
            cells.eq(1).html(price);
            cells.eq(2).html(time);

        }

        row = $stockTableBody.find("tr[datasymbol=\"" + symbol + "\"]");
            row.flash(up == false ? '255,148,148' // red
                        : '154,240,117', 500, 500);
    };

    $.connection.hub.start().done(function () {
        hub.server.startTicker();
    });
    });
