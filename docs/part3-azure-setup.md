# Part 3 — Azure Deployment + CI/CD Setup Guide
## PROG3176 Final Project — VehicleTrack API

---

## Step 1: Create Azure App Service (Free Tier)

1. Go to https://portal.azure.com
2. Search "App Services" → Create
3. Settings:
   - **Resource Group:** Create new → `vehicletrack-rg`
   - **Name:** `vehicletrack-api` (this becomes your URL)
   - **Runtime stack:** .NET 9
   - **OS:** Linux
   - **Region:** Canada Central (or East US)
   - **Plan:** Free F1
4. Click Review + Create → Create

Your public URL will be: `https://vehicletrack-api.azurewebsites.net`

---

## Step 2: Get the Publish Profile

1. In Azure Portal → Your App Service → Overview
2. Click **"Download publish profile"**
3. Open the downloaded `.PublishSettings` file in a text editor
4. Copy the entire contents

---

## Step 3: Add GitHub Secrets

In your GitHub repository:
1. Settings → Secrets and variables → Actions → New repository secret

Add these two secrets:

| Secret name            | Value                                      |
|------------------------|--------------------------------------------|
| `AZURE_APP_NAME`       | `vehicletrack-api`                         |
| `AZURE_PUBLISH_PROFILE`| (paste the full contents of publish profile) |

---

## Step 4: Push to main branch

```
git add .
git commit -m "ci: add GitHub Actions workflow for Azure deployment"
git push origin main
```

GitHub Actions will automatically:
1. Restore NuGet packages
2. Build in Release mode
3. Publish
4. Deploy to Azure App Service

Check progress at: GitHub repo → Actions tab

---

## Step 5: Verify deployment

Open in browser:
```
https://vehicletrack-api.azurewebsites.net/swagger
```

Screenshot the:
- Live Swagger UI at the Azure URL
- GitHub Actions workflow run showing green checkmark

---

## Notes

- The SQLite database file is created automatically on first startup
- If you see a 500 error, check App Service → Log stream for errors
- The free F1 tier sleeps after 20 mins of inactivity — first request may be slow
