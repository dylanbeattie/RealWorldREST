function setCookie(key, value) {
    var expires = new Date();
    expires.setTime(expires.getTime() + 1 * 24 * 60 * 60 * 1000);
    document.cookie = key + "=" + value + ";expires=" + expires.toUTCString();
}

function getCookie(key) {
    var keyValue = document.cookie.match("(^|;) ?" + key + "=([^;]*)(;|$)");
    return keyValue ? keyValue[2] : null;
}

(function explorerViewModel() {
    var self = this;

    this.hideSpinner = function (callback) { $("#spinner").fadeOut(callback); };
    this.showSpinner = function (callback) { $("#spinner").fadeIn(callback); };

    this.buildErrorHandler = function (method, absoluteUrl, callback) {
        return function (jqXHR, textStatus, errorThrown) {
            var headers = jqXHR.getAllResponseHeaders();
            var html = "<strong>" + method + " " + absoluteUrl + "</strong>\r\n\r\n";
            html += headers.replace(/access\-control.*\r\n/mg, "") + "\r\n";
            html += jqXHR.status + " " + textStatus + "\r\n\r\n";
            try {
                html += JSON
                    .stringify(JSON.parse(jqXHR.responseText), null, 2)
                    .replace(/\\r\\n/g, "\r\n")
                    .replace(/ --->/g, "\r\n    --->");
            } catch (error) {
                html += jqXHR.responseText;
            }
            $("#json-data").html(html);
            self.hideSpinner();
            if (typeof callback === "function") callback();
        };
    };

    this.buildSuccessHandler = function (method, absoluteUrl, callback) {
        return function (data, textStatus, jqXhr) {
            var headers = jqXhr.getAllResponseHeaders();
            var html = "<strong>" + method + " " + absoluteUrl + "</strong>\r\n\r\n";
            html += jqXhr.status + " " + textStatus + "\r\n\r\n";
            html += headers.replace(/access\-control.*\r\n/mg, "") + "\r\n";
            if (typeof data !== "undefined" && data !== null) {
                var putJson = JSON.stringify(simplify(data), null, 4);
                var actions = scan(data);
                html += hyperlink(data);
                for (var key in actions) {
                    if (!actions.hasOwnProperty(key)) continue;
                    console.log(key);
                    var action = actions[key];
                    var form = "<form class='api-form' id='form-" +
                        key.replace(/\./g, "-") +
                        "' method='" +
                        action.method +
                        "' action='" +
                        action.href +
                        "'>";
                    form += "<p><label>href:</label>" + action.href + "</p>";
                    form += "<p><label>method:</label>" + action.method + "</p>";
                    if (action.type) {
                        form += "<p><label>type:</label>" + action.type + "</p>";
                        form += "<p><label>body:</label> <textarea rows='10' cols='80'>";
                        if (action.method === "PUT") {
                            form += putJson;
                        }
                        form += "</textarea></p>";
                    }
                    form += "</form>";
                    html = html.replace(key,
                        "<a class='action-button " +
                        action.method +
                        "' href='#' data-action-name='" +
                        action.name +
                        "' data-form-id='form-" +
                        key.replace(/\./g, "-") +
                        "' id='link-" +
                        key +
                        "'>" +
                        key.split(".").pop() +
                        "</a>" +
                        form);
                }
            }
            $("#json-data").html(html);
            self.hideSpinner();
            if (typeof callback === "function") callback();
        };
    };

    this.loadResource = function (url, method, data, callback) {
        var absoluteUrl = $("#server-input").val() + url;
        var type = method || "GET";
        this.showSpinner(function () {
            $.ajax({
                headers: { Accept: $("#api-version-select").val() },
                url: absoluteUrl,
                type: type,
                data: data,
                contentType: "application/json",
                dataType: "json",
                beforeSend: addHeaders,
                success: buildSuccessHandler(type, absoluteUrl, callback),
                error: buildErrorHandler(type, absoluteUrl, callback)
            });

        });
    };

    this.simplify = function (data) {
        var result = new Object();
        for (var key in data) {
            if (!data.hasOwnProperty(key)) continue;
            if (/^_/.test(key)) continue; // Skip HAL+JSON hypermedia properties
            if (/href/i.test(key)) continue; // Skip any HREF properties.
            result[key] = data[key];
        }
        return result;
    };

    this.scan = function (data, path, hash) {
        console.log(path);
        hash = hash || new Object();
        path = path === undefined ? "" : path;
        for (var key in data) {
            if (!data.hasOwnProperty(key)) continue;
            var prefix = (path === "" ? path : path + ".") + key;
            if (key === "_actions") {
                for (var subkey in data[key]) {
                    if (!data[key].hasOwnProperty(subkey)) continue;
                    var id = prefix + "." + subkey;
                    data[key][id] = data[key][subkey];
                    if (data[key].hasOwnProperty(subkey)) hash[id] = data[key][subkey];
                    delete data[key][subkey];
                }
            } else if (typeof data[key] === "object") {
                scan(data[key], prefix, hash);
            }
        }
        return hash;
    };

    this.hyperlink = function (data) {
        var json = JSON.stringify(data, null, 2);
        var html = json.replace(/"href": "([^<].*)"/g, '"href": "<a href="#$1" class="api-link">$1</a>"');
        return html;
    };

    this.addHeaders = function (xhr) {
        var username = $("#username-input").val();
        var password = $("#password-input").val();
        if (username && password) xhr.setRequestHeader("Authorization", "Basic " + btoa(username + ":" + password));
    };

    this.go = function (url) {
        if (typeof url === "string") {
            $("#endpoint-input").val(url);
        } else {
            url = $("#endpoint-input").val();
        }
        location.hash = "#" + url;
        self.loadResource(url);
    };

    function handleForm() {
        var form = this;
        var $form = $(form);
        $form.dialog("close");
        var url = $form.attr("action");
        var method = $form.attr("method");
        var textarea = $form.find("textarea")[0];
        var data = null;
        if (textarea) {
            console.log($(textarea).val());
            var json = $form.find("textarea").val();
            data = json ? JSON.stringify(JSON.parse(json)) : null;
        }
        self.loadResource(url, method, data, function () { });
        return false;
    }

    $(function () {
        $("#username-input").change(function () { setCookie("explorer_username", this.value); });
        $("#password-input").change(function () { setCookie("explorer_password", this.value); });
        $("#server-input").change(function () { setCookie("explorer_server", this.value); });
        $("#explorer-form").submit(function () {
            go();
            return false;
        });

        $("#json-data").on("click", "a.action-button", function () {
            var $this = $(this);
            var formId = $this.data("form-id");
            var title = $this.data("action-name");
            var $form = $("#" + formId);
            $form.dialog({
                modal: true, width: 600, title: title,
                buttons: [
                    { text: "Submit", click: function () { $form.submit(); } },
                    { text: "Cancel", click: function () { $(this).dialog("close"); } }
                ]
            }).show();
            return false;
        });

        $(document).on("submit", "form.api-form", handleForm);

        $("#username-input").val(getCookie("explorer_username"));
        $("#password-input").val(getCookie("explorer_password"));
        var apiServer = getCookie("explorer_server") || "http://localhost";
        $("#server-input").val(apiServer);

        $("#go-button").click(go);
        $("#reset-button").click(function () {
            $("#json-data").html("Ready.");
            return (false);
        });

        $(window).on("hashchange", function () {
            go(location.hash.substring(1));
        });
        go(location.hash.substring(1) || "/");
        // self.loadResource(url);
    });
})();
