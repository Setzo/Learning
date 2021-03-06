
<%@ taglib uri="http://java.sun.com/jsp/jstl/core" prefix="c"%>

<c:import url="header.jsp">
<c:param name="title" value="Header"></c:param>
</c:import>

<nav class="navigation">
	<div class="select_users">
		<h2>
			<a href="#">Logged in user's name</a>
		</h2>
	</div>

	<ul class="users">
		<li><a href="#">List of other users</a></li>
		<li><a href="#">List of other users</a></li>
		<li><a href="#">List of other users</a></li>
	</ul>

	<ul class="admin">
		<li><a href="#">Manage users</a></li>
		<li><a href="#">Manage movies</a></li>
	</ul>
</nav>

<nav class="favs">
	<h2>Favourites</h2>
	<ul>
		<li><a href="#">Movie Title</a></li>
		<li><a href="#">Movie Title</a></li>
		<li><a href="#">Movie Title</a></li>
	</ul>
</nav>

<section class="movie_list">
	<h2>Hi, username</h2>
	<p class="welcome">Here you go, movies : Lorem ipsum dolor sit
		amet, consectetur adipisicing elit. Proin nibh augue, suscipit a,
		scelerisque sed, lacinia in, mi. Cras vel lorem. Etiam pellentesque
		aliquet tellus</p>

	<ul>
		<li>
			<figure>
				<a href="#"><img class="thumbnail" alt="Thumbnail"
					src="${pageContext.request.contextPath}/images/thumbnail.png"></a>
				<figcaption>
					<h3>
						<a href="#">Movie Title</a>
					</h3>
					<div class="description">Movie description</div>
				</figcaption>
			</figure>
		</li>
		<li>
			<figure>
				<a href="#"><img class="thumbnail" alt="Thumbnail"
					src="${pageContext.request.contextPath}/images/thumbnail.png"></a>
				<figcaption>
					<h3>
						<a href="#">Movie Title</a>
					</h3>
					<div class="description">Movie description</div>
				</figcaption>
			</figure>
		</li>
		<li>
			<figure>
				<a href="#"><img class="thumbnail" alt="Thumbnail"
					src="${pageContext.request.contextPath}/images/thumbnail.png"></a>
				<figcaption>
					<h3>
						<a href="#">Movie Title</a>
					</h3>
					<div class="description">Movie description</div>
				</figcaption>
			</figure>
		</li>
		<li>
			<figure>
				<a href="#"><img class="thumbnail" alt="Thumbnail"
					src="${pageContext.request.contextPath}/images/thumbnail.png"></a>
				<figcaption>
					<h3>
						<a href="#">Movie Title</a>
					</h3>
					<div class="description">Movie description</div>
				</figcaption>
			</figure>
		</li>
		<li>
			<figure>
				<a href="#"><img class="thumbnail" alt="Thumbnail"
					src="${pageContext.request.contextPath}/images/thumbnail.png"></a>
				<figcaption>
					<h3>
						<a href="#">Movie Title</a>
					</h3>
					<div class="description">Movie description</div>
				</figcaption>
			</figure>
		</li>
	</ul>
</section>

<c:import url="footer.jsp"></c:import>
