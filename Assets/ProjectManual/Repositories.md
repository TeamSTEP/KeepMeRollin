# Repositories

Unity's Collaboration function is mainly used for easily adding assets and syncing or collaborating with team members.
However, Project KMR will use Github as the main repository where project development progress is tracked and shared.
Unity Colaborate will be 
In short,

Github:

- Used for tracking milestones
- Commit major developments and improvements to the game to this repo
- Use branches for versioning and **only** merge to master when the
  version is completed. (In other words, branch versions should act as
  Alpha builds while the master is the late Beta or the release build)
- Used for submitting issues, project boards, etc.

Unity Collaboration:

- Used for uploading assets like music, art, graphics, etc. (This can
  be done via Github, but Collaboration is easier to use)
- Used for quickly downloading projects from Unity Hub
- Collaborate with team members
- Incremental and procedural updates to the current build
- Does not distinguish between Alpha or Beta versions
- Publishing changes to the project should match the commits to Github

Overall for developers, I recommend using Github for managing your development and committing your progress.
For collaborating with other developers, you can use a VS Code extension called [Live Share](https://marketplace.visualstudio.com/items?itemName=MS-vsliveshare.vsliveshare) or something similar in Visual Studio.

# Git Guide

## Branches and Forks

the basic idea of branches and PRs is, when implementing a new feature for most projects, you would create a branch that is only for that feature.
When the development is done you would open a pull request and merge it with the master.
We will elaborate on this idea and create a single standard as game development involves many moving parts that rely on each other and it is important for everyone to be on the same page with this.

There will be mainly three types of branches for KMR.

- `fix` branch: a branch name that is used for commits for patches or fixes that address a bug that came from the `feature` branch. This does not add anything new in terms of features, but only refactors them. Ex) `fix/throw-object`.
- `feature` branch: branches that adds something new to the game which may break the game or not. Because of how game development works, commits in this branch will inadvertently act similar as the `fix` branch (and it is fine to do that as long as you record it!), but one key difference is that `feature` branch must add something new. Ex) `feature/ai-sensors`.
- `development` branch: these are the branches that are in active development. They have a very strict rule. First, the name must be the semantic version name. Second, every `feature` and `fix` branches must be derived from this branch and merge back to it when finished (no branch should derive from other branches). Lastly, after merging the `development` branch with the `master` branch, the `development` branch should not be touched at all. Ex) `development/v-0.0.1b`

In short, branches with the name `development` must be derived from the `master` while the `fix` and `feature` branches derive from the latest `development` branch. Only `development` branches are allowed to merge with `master`. However, you can only merge when there are no `fix` or `feature` branches, and once you merge you can only create a new `development` branch, no direct commits to `master` **unless the situation calls for it**.

Forks works similar to the `fix` and `feature` branch.
You can freely fork a `development` branch and merge back to it. But you should never fork and commit to a `fix`/`feature` branch unless you made a new one!

## Versioning Scheme
By default, this project will use the **Semantic Versioning** as the base for the versioning scheme.

Versions are generally used in three different places.
- Github tag/dev branch names
- Unity game version
- In-game graphical version display

Unity game version and the In-game graphical version display will be linked together, allowing the developer to only focus on working with the in-engine version numbering.
Those will have to be changed manually.
Github version names will be taking the form of development branch names like `development/v0.1.3a`.
The naming scheme for branch names will be like the following,
- `development/v[major].[minor].[patch][alpha/beta]`.

The major number is when

## How Versions Increment
Version number 