"use strict";
const formatProductCode = value => {
    const payload = value.toUpperCase().replace(/[^A-Z0-9]/g, "").slice(0, 30);
    return payload.match(/.{1,5}/g)?.join("-") ?? "";
};

const productCodeInput = document.getElementById("productCode");
productCodeInput?.addEventListener("input", () => {
    productCodeInput.value = formatProductCode(productCodeInput.value);
});

const searchInput = document.getElementById("search");
searchInput?.addEventListener("input", () => {
    searchInput.value = formatProductCode(searchInput.value);
});
document.getElementById("qrModal").addEventListener("show.bs.modal", event => {
    const code = event.relatedTarget.dataset.code;
    const target = document.getElementById("qrCode");
    target.replaceChildren();
    document.getElementById("qrValue").textContent = code;
    new QRCode(target, { text: code, width: 256, height: 256, correctLevel: QRCode.CorrectLevel.M });
});
document.getElementById("deleteModal").addEventListener("show.bs.modal", event => {
    document.getElementById("deleteId").value = event.relatedTarget.dataset.id;
    document.getElementById("deleteValue").textContent = event.relatedTarget.dataset.code;
});
document.getElementById("deleteModal").addEventListener("hidden.bs.modal", () => {
    document.getElementById("deleteId").value = "";
});

