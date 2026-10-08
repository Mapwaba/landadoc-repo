# Putting the web apps on landadoc.cd

The three web apps run as three Vercel projects, at `landadoc-*.vercel.app`. This guide puts them under your own domain, one subdomain per app:

| App | Address | Vercel project |
|---|---|---|
| Patient | `landadoc.cd` (and `www.landadoc.cd`) | `landadoc-patient` |
| Doctor | `doctor.landadoc.cd` | `landadoc-doctor` |
| Admin | `admin.landadoc.cd` | `landadoc-admin` |

No code changes are needed. The work happens in three dashboards: Vercel, your domain registrar and Render.

## Why subdomains and not paths

Paths such as `landadoc.cd/doctor` and `landadoc.cd/admin` would put all three apps on one origin, which means:

- **They'd share browser storage.** The apps keep the login under the same name, so logging into the admin app in a browser would overwrite the patient login in that same browser.
- **More moving parts.** Each app would need to be rebuilt to run under a sub-path, and a fourth Vercel project would have to proxy requests to the other three.

Subdomains avoid both. The [Docker setup](docker-compose.prod.yml) uses subdomains for the same reason.

## 1. Add the domains in Vercel

In each project, open **Settings → Domains → Add**:

| Project | Domains to add |
|---|---|
| `landadoc-patient` | `landadoc.cd` and `www.landadoc.cd`. When Vercel offers to redirect `www` to the bare domain, accept. |
| `landadoc-doctor` | `doctor.landadoc.cd` |
| `landadoc-admin` | `admin.landadoc.cd` |

After you add a domain, Vercel shows the DNS record it expects. Keep that screen open for step 2.

## 2. Create the DNS records

At the registrar where `landadoc.cd` is registered, create:

| Type | Name | Value |
|---|---|---|
| A | `@` | the IP address Vercel shows (commonly `76.76.21.21`) |
| CNAME | `www` | the value Vercel shows |
| CNAME | `doctor` | the value Vercel shows |
| CNAME | `admin` | the value Vercel shows |

Copy the values from Vercel rather than from this table: Vercel sometimes gives each project its own CNAME value.

**If the registrar's DNS panel can't do this** (some `.cd` registrars offer only basic settings), point the domain's **nameservers** to Vercel instead. In Vercel, go to the account's **Domains** page, add `landadoc.cd`, and it shows the two nameservers to enter at the registrar. Vercel then creates the records itself.

DNS changes take from a few minutes to a few hours to spread. Once they do, each domain shows **Valid Configuration** in Vercel, and Vercel issues the HTTPS certificates automatically.

## 3. Let the new addresses call the API

The backend on Render only accepts browser calls from the addresses it knows. Without this step, the apps load but every request fails.

In Render, open **Environment Groups → landadoc-shared** and **add** these keys. Keep the existing `vercel.app` entries (`__0` to `__2`) for now, so both old and new addresses work during the switch:

| Key | Value |
|---|---|
| `Cors__AllowedOrigins__3` | `https://landadoc.cd` |
| `Cors__AllowedOrigins__4` | `https://www.landadoc.cd` |
| `Cors__AllowedOrigins__5` | `https://doctor.landadoc.cd` |
| `Cors__AllowedOrigins__6` | `https://admin.landadoc.cd` |

Then, on the **payment** service, set `Frontend__PatientBaseUrl` to `https://landadoc.cd`. Card payments (Stripe) send patients back to this address after they pay.

Redeploy the Render services so they pick up the new values: open each one, then **Manual Deploy → Deploy latest commit**.

## 4. Check

- Open `https://landadoc.cd`, `https://doctor.landadoc.cd` and `https://admin.landadoc.cd`, and log in to each.
- Search for a doctor and open their profile. The calendar should show open times, which proves the API accepts the new address.
- Make one card payment in test mode and check that Stripe brings you back to `landadoc.cd`.

If an app loads but shows errors or empty pages, open the browser's developer tools (F12) → **Console**. A message mentioning **CORS** means step 3 is missing that address, or Render wasn't redeployed.

## 5. Retire the old addresses (optional)

Once everyone uses the new addresses, remove `Cors__AllowedOrigins__0` to `__2` (the `vercel.app` ones) from the environment group and redeploy. The `vercel.app` addresses keep loading, but they can no longer talk to the API.

## Good to know

- **Everyone logs in once more.** Each address has its own browser storage, so logins on `vercel.app` don't carry over.
- **Use the new addresses everywhere.** Update any links you've shared, such as in emails, posters and QR codes, to the new addresses.
- **API addresses (optional, later).** The backend still answers at `landadoc-*.onrender.com`. Users never see this, but you could give the services addresses like `api.landadoc.cd` in each Render service's **Settings → Custom Domains**. If you do, also update [deploy/vercel/appsettings.Production.json](deploy/vercel/appsettings.Production.json) with the new API addresses.
