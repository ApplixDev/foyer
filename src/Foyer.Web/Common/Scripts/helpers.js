var App = App || {};
(function () {

    var appLocalizationSource = abp.localization.getSource('Foyer');
    App.localize = function () {
        return appLocalizationSource.apply(this, arguments);
    };

})(App);