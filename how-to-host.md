# Hosting on Render + MongoDB Atlas

## 1. MongoDB Atlas

1. Create a free account at https://mongodb.com/atlas
2. Create a free **M0** cluster (any region)
3. Create a database user (username + password)
4. Under **Network Access**, add `0.0.0.0/0` (allow all IPs — needed since Render free tier has no static IP)
5. Get your connection string — it will look like:
   ```
   mongodb+srv://username:password@cluster0.xxxxx.mongodb.net/tiles
   ```

## 2. Render

1. Push your repo to GitHub
2. Go to https://render.com → **New → Web Service**
3. Connect your GitHub repo
4. Render will detect the `Dockerfile` automatically — select **Docker** as runtime
5. Set **Port** to `8080`
6. Add an environment variable:
   - Key: `ConnectionStrings__MongoDb`
   - Value: your Atlas connection string from step 1
7. Deploy

## 3. Pre-generate tiles (recommended)

Before sharing the URL, run the console app locally pointed at Atlas to pre-generate zoom levels 0–5. This avoids slow on-demand tile generation on the free-tier CPU.

```bash
cd MandelbrotConsole
ConnectionStrings__MongoDb="mongodb+srv://..." dotnet run -- 5
```

## Notes

- The Atlas `0.0.0.0/0` network allowlist is required on the free tier because Render free services don't have a fixed outbound IP. Static IPs are a paid Render feature.
- Render free web services spin down after 15 minutes of inactivity. The first request after idle will be slow while the container restarts. Pre-generating tiles helps since cached tiles are served quickly even on a cold start.
