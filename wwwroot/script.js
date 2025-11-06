// HEADER SHRINK
window.addEventListener('scroll', function(){
  const header = document.querySelector('.header');
  if(window.scrollY > 50) header.classList.add('shrink');
  else header.classList.remove('shrink');
});

// NAV LINK SMOOTH SCROLL + ACTIVE
const navLinks = document.querySelectorAll('.nav-link');
navLinks.forEach(link => {
  link.addEventListener('click', e => {
    e.preventDefault();
    navLinks.forEach(l=>l.classList.remove('active'));
    link.classList.add('active');
    const targetId = link.getAttribute('href');
    const targetSection = document.querySelector(targetId);
    if(targetSection){
      const headerHeight = document.querySelector('.header').offsetHeight;
      const targetPosition = targetSection.offsetTop - headerHeight;
      window.scrollTo({top:targetPosition, behavior:'smooth'});
    }
  });
});

// ALUMNI CARD ANIMATION
const cards = document.querySelectorAll('.alumni-card');
function revealCards(){
  const triggerBottom = window.innerHeight * 0.85;
  cards.forEach(card=>{
    const cardTop = card.getBoundingClientRect().top;
    if(cardTop < triggerBottom) card.classList.add('visible');
  });
}
window.addEventListener('scroll', revealCards);
document.addEventListener('DOMContentLoaded', revealCards);
