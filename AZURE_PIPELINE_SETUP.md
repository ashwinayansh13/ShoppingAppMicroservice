# Azure Pipeline Setup Guide - Docker Hub

This guide explains how to configure and use the Azure Pipeline for SimpleProject microservices to publish Docker images to Docker Hub.

## Prerequisites

1. Azure DevOps account and organization
2. Docker Hub account
3. Docker Hub username and password (or access token)

## Pipeline Overview

The `azure-pipelines.yml` file includes three main stages:

1. **Build**: Builds the .NET solution and runs tests
2. **BuildDockerImages**: Builds Docker images for all three services
3. **PushImages**: Pushes images to Docker Hub (only on main branch)
4. **Deploy**: Optional deployment stage (commented out)

## Configuration Steps

### 1. Create Docker Hub Account (if you don't have one)

1. Go to [Docker Hub](https://hub.docker.com/)
2. Sign up for a free account
3. Verify your email address

### 2. Create Docker Hub Access Token (Recommended)

Using an access token is more secure than using your password:

1. Log in to Docker Hub
2. Go to **Account Settings** → **Security**
3. Click **New Access Token**
4. Give it a name (e.g., "Azure DevOps Pipeline")
5. Set permissions:
   - ✅ **Read, Write & Delete** (for full access)
   - Or **Read & Write** (for push/pull only)
6. Click **Generate**
7. **Copy the token immediately** (you won't see it again!)

### 3. Add Docker Hub Credentials to Azure DevOps

**Option A: Using Service Connection (Recommended)**

1. Go to your Azure DevOps project
2. Navigate to **Project Settings** → **Service connections**
3. Click **New service connection**
4. Select **Docker Registry** → **Docker Hub**
5. Fill in:
   - **Docker ID**: Your Docker Hub username
   - **Docker Password**: Your Docker Hub password or access token
   - **Service connection name**: `Docker Hub`
6. Click **Save**

**Option B: Using Pipeline Variables (Alternative)**

1. Go to your Azure DevOps project
2. Navigate to **Pipelines** → **Library**
3. Create a new **Variable group** (e.g., "Docker Hub Secrets")
4. Add variables:
   - **Name**: `DOCKERHUB_USERNAME`
   - **Value**: Your Docker Hub username
   - **Name**: `DOCKERHUB_PASSWORD`
   - **Value**: Your Docker Hub password or access token
   - ✅ **Keep this value secret** (for password)
5. In your pipeline YAML, reference the variable group:
   ```yaml
   variables:
     - group: 'Docker Hub Secrets'
   ```

### 4. Update Pipeline Variables

Edit `azure-pipelines.yml` and update this variable:

```yaml
variables:
  dockerHubUser: 'YOUR_DOCKERHUB_USERNAME'  # Your Docker Hub username
  containerRegistry: 'docker.io'  # Docker Hub registry URL
  imageRepository: 'simpleproject'  # Base repository name
```

**Example:**
```yaml
variables:
  dockerHubUser: 'johndoe'  # Your Docker Hub username
  containerRegistry: 'docker.io'
  imageRepository: 'simpleproject'
```

This will create images like:
- `johndoe/simpleproject-galleryservice:latest`
- `johndoe/simpleproject-checkoutservice:latest`
- `johndoe/simpleproject-webgateway:latest`

### 5. Configure Pipeline in Azure DevOps

1. Go to **Pipelines** → **Pipelines**
2. Click **New pipeline**
3. Select your repository (Azure Repos, GitHub, etc.)
4. Choose **Existing Azure Pipelines YAML file**
5. Select `azure-pipelines.yml` from the root directory
6. If using Option B (variable group), add it in the pipeline settings:
   - Click **Variables** → **Variable groups**
   - Link your variable group
7. Click **Run**

## Pipeline Triggers

The pipeline triggers on:
- **Branches**: `main`, `develop`
- **Pull Requests**: To `main` or `develop`
- **Paths**: Changes in `Services/`, `Shared/`, or solution/project files

## Stages Explained

### Build Stage
- Installs .NET 8.0 SDK
- Restores NuGet packages
- Builds the solution
- Runs tests (if test projects exist)

### BuildDockerImages Stage
- Builds Docker images for:
  - GalleryService
  - CheckoutService
  - WebGateway
- Tags images with build ID and `latest`
- Image format: `{username}/{repo}-{service}:{tag}`

### PushImages Stage
- Only runs on `main` branch
- Pushes images to Docker Hub
- Tags: `{BuildId}-{service}` and `latest`
- Images are automatically visible in your Docker Hub repositories

### Deploy Stage (Optional)
- Currently commented out
- Example deployment to Azure Container Instances
- Can be modified for:
  - Azure Kubernetes Service (AKS)
  - Azure App Service
  - Azure Container Apps

## Viewing Published Images

After the pipeline runs successfully:

1. Go to [Docker Hub](https://hub.docker.com/)
2. Log in to your account
3. Click on **Repositories**
4. You'll see your Docker images:
   - `simpleproject-galleryservice`
   - `simpleproject-checkoutservice`
   - `simpleproject-webgateway`

## Pulling Images from Docker Hub

To pull images from Docker Hub:

```bash
# Login to Docker Hub (if needed)
docker login

# Pull images
docker pull YOUR_USERNAME/simpleproject-galleryservice:latest
docker pull YOUR_USERNAME/simpleproject-checkoutservice:latest
docker pull YOUR_USERNAME/simpleproject-webgateway:latest
```

## Making Images Public (Optional)

By default, images are private (if you have a free account, you get one private repo). To make them public:

1. Go to Docker Hub → **Repositories**
2. Click on a repository
3. Click **Settings**
4. Scroll down to **Repository Visibility**
5. Click **Make Public**

**Note**: Free Docker Hub accounts have:
- Unlimited public repositories
- 1 private repository
- Consider upgrading for more private repos

## Local Testing

Test the pipeline locally using Azure DevOps CLI:

```bash
# Install Azure DevOps CLI extension
az extension add --name azure-devops

# Login
az devops login

# Run pipeline validation
az pipelines validate --org https://dev.azure.com/yourorg --project yourproject --path azure-pipelines.yml
```

## Troubleshooting

### Build Fails
- Check .NET SDK version matches (8.0)
- Verify all project references are correct
- Ensure SharedModels project builds first

### Docker Build Fails
- Verify Dockerfile paths are correct
- Check build context includes all necessary files
- Ensure SharedModels is accessible

### Push Fails - Authentication Error
- Verify Docker Hub credentials are correct
- Check service connection is configured correctly
- Ensure username and password/token are correct
- Try regenerating the access token

### Push Fails - Permission Denied
- Verify Docker Hub username is correct
- Check if you've reached your private repository limit (free accounts: 1 private repo)
- Make repositories public if needed
- Ensure access token has write permissions

### Push Fails - Rate Limit
- Docker Hub free accounts have rate limits:
  - **Anonymous**: 100 pulls per 6 hours per IP
  - **Authenticated**: 200 pulls per 6 hours
- Consider upgrading to a paid plan for higher limits
- Use pull-through caches for CI/CD

### Images Not Visible in Docker Hub
- Wait a few minutes for Docker Hub to process
- Check if images are private and you're logged in
- Verify the push stage completed successfully
- Check Docker Hub for any error messages

## Environment Variables

For deployment, you may need to set:
- `DOCKERHUB_USERNAME`: Docker Hub username (stored as secret)
- `DOCKERHUB_PASSWORD`: Docker Hub password or access token (stored as secret)

## Security Best Practices

1. **Never commit credentials to code** - Always use Azure DevOps secrets/variables
2. **Use access tokens** - Prefer access tokens over passwords
3. **Rotate tokens regularly** - Update tokens periodically
4. **Use variable groups** - Store secrets in Azure DevOps variable groups
5. **Limit token permissions** - Only grant necessary permissions
6. **Review repository access** - Control who can access your images

## Docker Hub Rate Limits

Be aware of Docker Hub rate limits:

| Account Type | Pull Limit |
|-------------|------------|
| Anonymous | 100 pulls per 6 hours per IP |
| Free (Authenticated) | 200 pulls per 6 hours |
| Pro/Team | Unlimited |

Consider:
- Using pull-through caches
- Upgrading to a paid plan
- Using alternative registries for CI/CD

## Next Steps

1. Create Docker Hub account (if needed)
2. Create Docker Hub access token
3. Configure service connection or variable group in Azure DevOps
4. Update `dockerHubUser` variable in `azure-pipelines.yml`
5. Run the pipeline
6. Verify images appear in Docker Hub
7. Configure deployment stage if needed
8. Set up environment-specific configurations

## Additional Resources

- [Docker Hub Documentation](https://docs.docker.com/docker-hub/)
- [Docker Hub Pricing](https://www.docker.com/pricing/)
- [Azure DevOps Docker Task](https://docs.microsoft.com/en-us/azure/devops/pipelines/tasks/build/docker)
- [Docker Hub Rate Limits](https://www.docker.com/increase-rate-limits)
