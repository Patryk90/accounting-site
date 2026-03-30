document.addEventListener("submit", async function (e) {
    const form = e.target.closest("form[data-spa-form]");
    if (!form) return;

    e.preventDefault();

    const formData = new FormData(form);

    const response = await fetch(location.href, { method: "POST", body: formData });
    const html = await response.text();
    const parser = new DOMParser();
    const doc = parser.parseFromString(html, "text/html");

    const newForm = doc.querySelector("#contact-form-wrapper");
    if (newForm) {
        document.querySelector("#contact-form-wrapper").innerHTML = newForm.innerHTML;
        document.querySelector("#contact-form-wrapper").scrollIntoView({ behavior: "smooth" });
    }
});