define(["loading", "emby-button", "emby-select"],
    function (loading) {

        return function(view) {
            view.addEventListener('viewshow', () => {
                const page = view;

                loading.show();

                var FrontViewConfigurationPage = {
                    pluginUniqueId: "9574ac10-bf23-49bc-1111-924f23cfa48f"
                };

                const setDeviceList = function (config) {
                    const deviceNameEl = page.querySelector("#deviceName");
                    if (Array.isArray(config.DevicesRunning)) {
                        config.DevicesRunning.forEach(function (c) {
                            let option = document.createElement("option");
                            option.text = c.Client + '  : Running on:  ' + c.DeviceName + ' with User:  ' + c.DeviceId;
                            option.value = c.Id;
                            deviceNameEl.add(option);
                        });
                    } else {
                        console.error('Invalid or undefined DevicesRunning');
                    }
                };


                ApiClient.getPluginConfiguration(FrontViewConfigurationPage.pluginUniqueId).then(function (config) {
                    setDeviceList(config);
                    loading.hide();
                });

                const configFormSubmitListener = function(e) {
                    e.stopPropagation();
                    e.preventDefault();

                    loading.show();
                    const form = this;

                    ApiClient.getPluginConfiguration(FrontViewConfigurationPage.pluginUniqueId).then(function (config) {
                        config.SelectedDeviceId = form.querySelector("#deviceName").value; //$('#deviceName option:selected').val();
                        ApiClient.updatePluginConfiguration(FrontViewConfigurationPage.pluginUniqueId, config).then(Dashboard.processPluginConfigurationUpdateResult);
                    }).finally(function() {
                        loading.hide();
                    });

                };

                const refreshDeviceListListener = function(e) {
                    e.stopPropagation();
                    e.preventDefault();

                    loading.show();

                    const deviceNameEl = page.querySelector("#deviceName");
                    deviceNameEl.options.length = 0;

                    ApiClient.getPluginConfiguration(FrontViewConfigurationPage.pluginUniqueId).then(function (config) {
                        setDeviceList(config);
                    }).finally(function() {
                        loading.hide();
                    });

                };

                const configFormEl = page.querySelector(".FrontViewConfigurationForm");
                configFormEl.addEventListener("submit", configFormSubmitListener, true);


                const refreshBtnEl = page.querySelector("#refreshDeviceList");
                refreshBtnEl.addEventListener("click", refreshDeviceListListener, true);
            });
        }
    }
);
