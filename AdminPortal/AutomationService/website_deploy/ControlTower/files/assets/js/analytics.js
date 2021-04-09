$('head').append('<script async src="https://www.googletagmanager.com/gtag/js?id=UA-90716026-1"></script>');

	window.dataLayer = window.dataLayer || [];
	  function gtag(){dataLayer.push(arguments);}
	  gtag('js', new Date());

	  gtag('config', 'UA-90716026-1');
	  gtag('config', 'AW-974298827');
	  
	  // [ buynow button ] start
	  


function gtag_report_conversion() {
	var url = 'https://1.envato.market/c/1289604/275988/4415?subId1=phoenixcoded&u=https%3A%2F%2Fthemeforest.net%2Fitem%2Fable-pro-responsive-bootstrap-4-admin-template%2F19300403';
	var callback = function () { 
		if (typeof(url) != 'undefined') { 
			 //window.location = url; 
		} 
	}; 
	gtag('event', 'conversion', { 'send_to': 'AW-974298827/HOSOCKjvoJMBEMu9ytAD', 'transaction_id': '', 'event_callback': callback }); 
	//return false;
}

$('body').append('' +
    '<div class="fixed-button active">' +
        '<a onclick="return gtag_report_conversion()" href="https://1.envato.market/c/1289604/275988/4415?subId1=phoenixcoded&u=https%3A%2F%2Fthemeforest.net%2Fitem%2Fable-pro-responsive-bootstrap-4-admin-template%2F19300403" target="_blank" class="btn" title="Buy Able Pro Admin Template Now On themeforest.net">' +
            '<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" version="1.1" id="Capa_1" x="0px" y="0px" viewBox="0 0 512.16 512.16" style="enable-background:new 0 0 512.16 512.16;" xml:space="preserve" width="15px" height="15px" class=""><g><path d="M428.669,22.858l-3.029-6.101C419.692,5.442,407.467-1.126,394.749,0.16  c-75.456,0-249.344,116.608-265.451,302.763c-17.138-41.848-24.984-86.918-22.997-132.096c0.004-8.828-5.615-16.678-13.973-19.52  c-8.423-2.955-17.794-0.259-23.36,6.72c-31.163,40.288-47.907,89.867-47.552,140.8c0,102.741,73.429,213.333,234.667,213.333  c173.205,0,234.667-137.899,234.667-256C489.945,174.417,468.597,94.19,428.669,22.858z" data-original="#4CAF50" class="active-path" data-old_color="#ffffff" fill="#ffffff"/></g> </svg> Buy Now!' +
        '</a> ' +
    '</div>' +
'');

$('head').append('' +
    '<style>' +
        '.fixed-button {position: fixed;bottom: -50px;right: 30px;-webkit-box-shadow: 0 13px 21px rgba(0, 0, 0, 0.15);box-shadow: 0 13px 21px rgba(0, 0, 0, 0.15);opacity: 0;z-index: 9;border-radius: 4px;-webkit-transition: all 0.5s ease;transition: all 0.5s ease;}.fixed-button .btn {margin: 0;background: #79b530;text-decoration:none;text-transform: capitalize;font-size: 15px;padding: 10px 19px;color: #fff;border-radius: 4px;border-bottom: 2px solid #5e8d25;}.fixed-button.active {bottom: 50px;opacity: 1;}' +
    '</style>' +
'');