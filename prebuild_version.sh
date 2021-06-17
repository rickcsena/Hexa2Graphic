# Remove old data
file="$(echo $1 | sed 's/\"//g')\GitVersion.cs"
rm -f "$file"

# Create GitVersion.cs file for version/release information
HASH=$(git rev-parse HEAD)
HASH_SHORT=$(git rev-parse --short HEAD)
BRANCH=$(git rev-parse --abbrev-ref HEAD)
COUNT_RELEASE=$(git rev-list --count --first-parent HEAD)
# TFS-GIT
#COUNT_PULL_REQUEST=$(git log --grep="PR [0-99999]:*" --pretty=oneline | sed 's/://g' | wc -l)
# GITHUB
COUNT_PULL_REQUEST=$(git log --grep="pull request #[0-99999]:*" --pretty=oneline | sed 's/://g' | wc -l)
cat > "$file" <<_EOF
/* ========================================
 * GitVersion.cs
 *
 * Version/Release information from git repo
 *
 * Copyright rickcsena@yahoo.com.br, 2021
 *
 * ========================================
 *
 * ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
 * +++++         Do not edit manually this file           +++++
 * +++++   it is automatically genarated on pré-build     +++++
 * ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
 *
*/
public static class GitVersion
{
    public static readonly string HASH_STRING = "$HASH";
    public static readonly string HASH_SHORT = "$HASH_SHORT";
    public static readonly string BRANCH = "$BRANCH";
    public static readonly string COUNT_RELEASE = "$COUNT_RELEASE";
    public static readonly string COUNT_PULL_REQUEST = "$COUNT_PULL_REQUEST";
}
_EOF
